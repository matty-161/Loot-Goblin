using Godot;
using System;

namespace LootGoblin;

[GlobalClass]
public partial class EnemyStatsComponent : StatsComponent
{
    // [Export] private EnemyStats EnemyStatsRes { get; set; }

    // combat
    [Export] public int Damage { get; private set; }
    [Export] public float AttackRange { get; private set; }
    // idle state
    [Export] public float IdleWaitTime { get; private set; }
    [Export] public float IdleChaseTriggerDistance { get; private set; }
    // patrol state
    [Export] public float PatrolTargetBufferDistance { get; private set; }
    [Export] public float PatrolChaseTriggerDistance { get; private set; }
    // chase state
    [Export] public float ChaseQuitDistance { get; private set; }
    // drop chances
    [Export] public float HealthDropChance { get; private set; }


    public override void _Ready()
    {
        MaxHealth = (int)(MaxHealth * DifficultyManager.DifficultyScale);
        Damage = (int)(Damage * DifficultyManager.DifficultyScale);
        MoveSpeed = (float)(MoveSpeed * DifficultyManager.DifficultyScale);
    }
}
