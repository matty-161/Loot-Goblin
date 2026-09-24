using Godot;
using System;

namespace LootGoblin;

public partial class PlayerStatsComponent : StatsComponent
{
    [Export] public float AttackCooldown { get; protected set; }
}
