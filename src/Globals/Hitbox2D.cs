using Godot;

namespace LootGoblin;

[GlobalClass]
public partial class Hitbox2D : Area2D
{
	public int Damage;
	
	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		// remove default collision settings
		SetCollisionLayerValue(1, false);
		SetCollisionMaskValue(1, false);
		
		Monitorable = false;
		Monitoring = false;

		if (Owner.IsInGroup("player"))
		{
			SetCollisionMaskValue(11, true);
			SetCollisionLayerValue(8, true);
		}
		else if (Owner.IsInGroup("enemies"))
		{
			SetCollisionMaskValue(9, true);
			SetCollisionLayerValue(10, true);
		}
	}
}