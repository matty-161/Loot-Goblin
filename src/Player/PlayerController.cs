using Godot;
using System;

namespace LootGoblin.Player;

public partial class PlayerController : CharacterBody2D
{
	[Export] private float _speed = 100f;
	
	private AnimatedSprite2D _sprite;

	private Vector2 _previousDirection = Vector2.Zero;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("%AnimatedSprite2D");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 movementInput = Input.GetVector(
			"move_left", "move_right", 
			"move_up", "move_down");

		if (movementInput != Vector2.Zero)
		{
			if (movementInput.X < 0) _sprite.FlipH = true;
			else if (movementInput.X > 0) _sprite.FlipH = false;
			_sprite.Play("walk");
		}
		else _sprite.Play("idle");
		
		_previousDirection = movementInput;
		
		Velocity = movementInput * _speed;
		MoveAndSlide();
	}
	
}
