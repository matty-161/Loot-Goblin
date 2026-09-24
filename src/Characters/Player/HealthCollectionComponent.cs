using Godot;
using System;

namespace LootGoblin;
public partial class HealthCollectionComponent : Area2D
{
	[Export] private HealthComponent _healthComponent;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is HealthDrop healthDrop)
		{
			_healthComponent.Heal(healthDrop.HealAmount);
			healthDrop.QueueFree();
		}
	}
}
