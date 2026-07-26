using System;
using Godot;

namespace LootGoblin.Autoloads;

public partial class GameplaySignalBus : Node
{
    public static GameplaySignalBus Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }
    
    // loot events
    public Action<int> LootChangedEvent;
    public Action<int> LootCollectedEvent;
}