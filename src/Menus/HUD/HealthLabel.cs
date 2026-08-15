using Godot;
using System;

namespace LootGoblin;

public partial class HealthLabel : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameplaySignalBus.Instance.PlayerHealthChangedEvent += OnPlayerHealthChanged;
	}

	private void OnPlayerHealthChanged(int newHealth)
	{
		Text = "Health: " + newHealth;
	}
	
}
