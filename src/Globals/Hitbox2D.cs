using Godot;

namespace LootGoblin.Globals;

[GlobalClass]
public partial class Hitbox2D : Area2D
{
	public int Damage;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CollisionMask = 6;
		CollisionLayer = 5;
	}
}