using Godot;
using System;
namespace LootGoblin.LevelGeneration;

public partial class ObserverCamera : Camera2D
{
	[Export] private float _speed = 5f;
	[Export] private float _zoomSpeed = 0.02f;
	
	public override void _PhysicsProcess(double delta)
	{
		// camera movement
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Position += direction * _speed;

		// camera zoom
		bool cameraZoomIn = Input.IsActionJustReleased("camera_zoom_in");
		bool cameraZoomOut = Input.IsActionJustReleased("camera_zoom_out");
		if (cameraZoomIn) Zoom += new Vector2(1, 1) * _zoomSpeed;
		if (cameraZoomOut) Zoom -= new Vector2(1, 1) * _zoomSpeed;
	}
}
