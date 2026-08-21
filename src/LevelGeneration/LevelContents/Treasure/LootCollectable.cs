using Godot;

namespace LootGoblin;

public partial class LootCollectable : Area2D
{
	[Export] public LootResource LootResource;
	
	[Export] public float CollectionTime { get; set; }

	public override void _EnterTree()
	{
		// remove default collision settings
		SetCollisionLayerValue(1, false);
		SetCollisionMaskValue(1, false);
		
		// set loot layer
		SetCollisionLayerValue(3, true);
	}
}