using Godot;
using System;

namespace LootGoblin;

public partial class LootLabel : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameEventManager.LootChangedEvent += OnLootChangedEvent;
	}

	public override void _ExitTree()
	{
		GameEventManager.LootChangedEvent -= OnLootChangedEvent;
	}

	private void OnLootChangedEvent(int amount)
	{
		Text = amount.ToString();
	}
}