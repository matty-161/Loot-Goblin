using Godot;

namespace LootGoblin;

public abstract partial class StatsComponent : Node
{
    [Export] public int MaxHealth { get; protected set; }
}