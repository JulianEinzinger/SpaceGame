using Godot;
using System;

public sealed partial class Laser : Node2D
{
	private static float? _destructionY;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_destructionY ??= CalcDestructionY();

		return;

		float CalcDestructionY()
		{
			var sprite = GetNode<Sprite2D>("Collision/LaserSprite");
			
			var textureHeight = sprite.Texture.GetSize().Y * Scale.Y;

			return -1 * (textureHeight * 0.5F + 1F);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Position.Y < _destructionY)
		{
			QueueFree();
		}
	}
}
