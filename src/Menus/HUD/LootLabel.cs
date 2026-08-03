using Godot;

namespace LootGoblin;

public partial class LootLabel : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameplaySignalBus.Instance.LootChangedEvent += OnLootChangedEvent;
	}

	public override void _ExitTree()
	{
		GameplaySignalBus.Instance.LootChangedEvent -= OnLootChangedEvent;
	}

	private void OnLootChangedEvent(int amount)
	{
		Text = "Loot: " + amount;
	}
}