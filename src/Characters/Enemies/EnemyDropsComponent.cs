using Godot;
using System;
using LootGoblin;

[GlobalClass]
public partial class EnemyDropsComponent : Node
{
    [Export] private HealthComponent _healthComponent;
    [Export] private EnemyStatsComponent _statsComponent;

    public override void _Ready()
    {
        _healthComponent.DiedEvent += OnDiedEvent;
    }

    private void OnDiedEvent()
    {
        var healthDropChance = _statsComponent.HealthDropChance;

        if (GD.Randf() < healthDropChance)
        {
            GameEventManager.DropHealthEvent?.Invoke(GetOwner<Node2D>().Position);
        }
    }
    
}
