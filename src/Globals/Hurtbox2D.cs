using Godot;

namespace LootGoblin.Globals;

[GlobalClass]
public partial class Hurtbox2D : Area2D
{
	[Signal] public delegate void TakeDamageEventHandler(int amount);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CollisionMask = 5;
		CollisionLayer = 6;

		AreaEntered += (Area2D area) =>
		{
			if (area is Hitbox2D hitbox)
			{
				EmitSignalTakeDamage(hitbox.Damage);
			}
		};
	}
}