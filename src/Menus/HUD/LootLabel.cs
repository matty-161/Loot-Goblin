using Godot;
using LootGoblin.Autoloads;

namespace LootGoblin.Menus.HUD;

public partial class LootLabel : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameplaySignalBus.Instance.LootChanged += (int amount) =>
		{
			Text = "Loot: " + amount;
		};
	}
}