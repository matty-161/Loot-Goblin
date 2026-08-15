using System;
using Godot;

namespace LootGoblin;

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
    
    // level events
    public Action LevelBossDiedEvent;
    public Action LevelTransitionFadeOutEvent;
    public Action LevelTransitionEvent;
    public Action<bool> LevelTransitionFadeInEvent;
    
    // player events
    public Action<int> PlayerHealthChangedEvent;
    public Action PlayerDied;
}