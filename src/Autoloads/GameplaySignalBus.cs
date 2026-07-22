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
    [Signal] public delegate void LootChangedEventHandler(int amount);
    [Signal] public delegate void LootCollectedEventHandler(int amount);
}