using Godot;

namespace LootGoblin;

public abstract partial class StatsComponent : Node
{
    [Export] public int MaxHealth { get; protected set; }
    [Export] public float AttackCooldown { get; protected set; }
    // movement
    [Export] public float MoveSpeed { get; protected set; }
    
    
}