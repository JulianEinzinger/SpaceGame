using Godot;
using System;
using FirstProject.Scripts;

public sealed partial class RotationController : ParentBehaviour
{
	[Export]
	private float _rotationSpeed = 20f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Calculate degreees/radians per Second
		var degreesPerSecond = _rotationSpeed * delta;
		var radiansPerSecond = (float) Mathf.DegToRad(degreesPerSecond);
		
		// Rotate the parent of the controller
		Parent?.Rotate(radiansPerSecond);
	}
}
