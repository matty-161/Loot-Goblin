using System;
using Godot;

namespace LootGoblin;

[GlobalClass]
public partial class Hurtbox2D : Area2D
{
	public Action<int> TakeDamageEvent;
	
	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		// remove default collision settings
		SetCollisionLayerValue(1, false);
		SetCollisionMaskValue(1, false);
		
		if (Owner.IsInGroup("player"))
		{
			SetCollisionMaskValue(10, true);
			SetCollisionLayerValue(9, true);
		}
		else if (Owner.IsInGroup("enemies"))
		{
			SetCollisionMaskValue(8, true);
			SetCollisionLayerValue(11, true);
		}

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