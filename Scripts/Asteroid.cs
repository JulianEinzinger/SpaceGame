using Godot;
using System;
using FirstProject.Scripts;

public sealed partial class Asteroid : Node2D
{
	private Sprite2D? _mainSprite;
	private AnimatedSprite2D? _explosionSprite;
	private bool _exploding;

	public override void _Ready()
	{
		// Get main + animated Sprite
		_mainSprite = this.GetNodeOrThrow<Sprite2D>("Collision/Sprite");
		_explosionSprite = this.GetNodeOrThrow<AnimatedSprite2D>("Explosion");
		
		_exploding = false;
		_explosionSprite.Hide();
		
		_explosionSprite.AnimationFinished += HandleExplosionFinished;
	}


	private async void OnCollision(Area2D other)
	{
		try
		{
			if (_exploding || other.Owner is not Laser laser)
			{
				return;
			}

			laser.Free();

			_exploding = true;
			_explosionSprite?.Show();
			_explosionSprite?.Play();
			
			// Stop movement
			this.GetNodeOrThrow<LinearMovementController>("Movement")
				.StopMoving();
			// Play sound
			this.GetNodeOrThrow<AudioStreamPlayer2D>("ExplosionSound")
				.Play();

			await ToSignal(GetTree().CreateTimer(1D),
				SceneTreeTimer.SignalName.Timeout);

			_mainSprite?.Hide();
		}
		catch (Exception e)
		{
			GD.Print($"Error in {nameof(OnCollision)}: {e.Message}");
		}
	}

	private void HandleExplosionFinished()
	{
		QueueFree();
	}

	public override void _ExitTree()
	{
		if (_explosionSprite is not null)
		{
			_explosionSprite.AnimationFinished -= HandleExplosionFinished;
		}
		
		base._ExitTree();
	}
}
