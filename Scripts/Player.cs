using Godot;
using System;

public sealed partial class Player : Sprite2D
{
	private PlayerBoundaries _boundaries;
	
	[Export]
	private float _maxY = 580F;
	[Export]
	private float _minY = 400F;
	[Export]
	private float _startX = 570F;
	[Export]
	private float _startY = 520F;
	[Export]
	private float _speed = 90F;
	[Export]
	private PackedScene? _laserPrefab;
	[Export] 
	private Vector2 _laserSpawnOffset = Vector2.Up * 20F;
	private AudioStreamPlayer2D? _laserSound;
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Position = new Vector2(_startX, _startY);
		_boundaries = DeterminePlayerBoundaries();
		
		// Grab the laser sound node
		_laserSound = GetNode<AudioStreamPlayer2D>("LaserSound");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Move(delta);
		Shoot();
	}

	private void Shoot()
	{
		const string ShootInput = "shoot";

		if (!Input.IsActionJustPressed(ShootInput))
		{
			return;
		}

		if (_laserPrefab is null)
		{
			throw new InvalidOperationException("Laser prefab is not set");
		}

		// Instantiate laser prefab
		var laser = _laserPrefab.Instantiate<Laser>();
		laser.Position = Position + _laserSpawnOffset;
		GetParent().AddChild(laser);
		_laserSound?.Play(); // Play laser sound
	}

	
	private void Move(double delta)
	{
		const string UpInput = "move_up";
		const string DownInput = "move_down";
		const string LeftInput = "move_left";
		const string RightInput = "move_right";

		var movementDirection = Vector2.Zero;
		if (Input.IsActionPressed(UpInput) && Position.Y > _boundaries.MinY)
		{
			movementDirection += Vector2.Up;
		}
		if (Input.IsActionPressed(DownInput) && Position.Y < _boundaries.MaxY)
		{
			movementDirection += Vector2.Down;
		}
		if (Input.IsActionPressed(LeftInput))
		{
			movementDirection += Vector2.Left;
		}
		if (Input.IsActionPressed(RightInput))
		{
			movementDirection += Vector2.Right;
		}

		// If all keys cancel each other out, don't translate (#efficiency)
		if (movementDirection == Vector2.Zero)
		{
			return;
		}
		
		Translate(movementDirection * _speed * (float)delta);
		
		if (Position.X < _boundaries.MinX)
		{
			Position = new Vector2(_boundaries.MaxX, Position.Y);
		} else if (Position.X > _boundaries.MaxX)
		{
			Position = new Vector2(_boundaries.MinX, Position.Y);
		}
	}

	private PlayerBoundaries DeterminePlayerBoundaries()
	{
		// does not handle resize during gam - initial values only
		Vector2 windowSize = GetViewportRect().Size;
		Vector2 textureSize = Texture.GetSize();
		Vector2 scaledSize = textureSize * Scale;
		Vector2 halfSize = scaledSize / 2F;
		
		return new PlayerBoundaries(halfSize.X, 
									windowSize.X - halfSize.X, 
									_minY, 
									_maxY);
	}
	
	private readonly record struct PlayerBoundaries(float MinX, float MaxX, 
													float MinY, float MaxY);
}
