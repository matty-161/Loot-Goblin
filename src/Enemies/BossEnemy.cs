using System;
using Godot;
using LootGoblin.Autoloads;
using LootGoblin.Globals;

namespace LootGoblin.Enemies;

public partial class BossEnemy : CharacterBody2D, IDamageable
{
	
	[Export] public int Health { get; set; }
	[Export] public Hurtbox2D Hurtbox { get; set; }

	public override void _Ready()
	{
		Hurtbox.TakeDamageEvent += TakeDamage;
	}

	public override void _ExitTree()
	{
		Hurtbox.TakeDamageEvent -= TakeDamage;
	}

	public void TakeDamage(int amount)
	{
		Health -= amount;
		if (Health <= 0) OnDeath();
	}

	public void OnDeath()
	{
		GameplaySignalBus.Instance.LevelBossDiedEvent?.Invoke();
		QueueFree();
	}
}