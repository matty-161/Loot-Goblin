using System;
using Godot;

namespace LootGoblin;

[GlobalClass]
public partial class Hurtbox2D : Area2D
{
	public Action<int> TakeDamageEvent;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CollisionMask = 5;
		CollisionLayer = 6;

		AreaEntered += OnAreaEntered;
	}

	public override void _ExitTree()
	{
		AreaEntered -= OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is Hitbox2D hitbox)
		{
			TakeDamageEvent?.Invoke(hitbox.Damage);
		}
	}
}