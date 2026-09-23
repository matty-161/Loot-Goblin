using Godot;
using System;

namespace LootGoblin;

[GlobalClass]
public partial class EnemyStats : Resource
{
    [ExportGroup("Combat")]
    [Export] public int MaxHealth;
    [Export] public int Damage;
    [Export] public float AttackRange;

    [ExportGroup("Movement")]
    [Export] public float MoveSpeed;
    
    [ExportGroup("Idle")]
    [Export] public float IdleWaitTime;
    [Export] public float IdleChaseTriggerDistance;
    
    [ExportGroup("Patrol")]
    [Export] public float PatrolTargetBufferDistance;
    [Export] public float PatrolChaseTriggerDistance;
    
    [ExportGroup("Chase")]
    [Export] public float ChaseQuitDistance;
    
    [ExportGroup("Attack")]
    [Export] public float AttackCooldown;

}
