using Godot;
using System;
using LootGoblin.Autoloads;
using LootGoblin.LevelGeneration.LevelContents.Treasure;

namespace LootGoblin.Player;

public partial class LootCollectionComponent : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is not LootCollectable collectable) return;
		GameplaySignalBus.Instance.LootCollectedEvent?.Invoke(collectable.LootResource.Amount);
		collectable.QueueFree();
	}
}
