using Godot;
using System;
using LootGoblin.Autoloads;

namespace LootGoblin.Systems;

public partial class LootManager : Node
{
    private int _lootTotal = 0;

    public int LootTotal
    {
        get => _lootTotal;
        set
        {
            _lootTotal = value;
            GameplaySignalBus.Instance.EmitSignal("LootChanged", _lootTotal);
        }
    }

    public override void _Ready()
    {
        GameplaySignalBus.Instance.LootCollectedEvent += OnLootCollectedEvent;
    }

    public override void _ExitTree()
    {
        GameplaySignalBus.Instance.LootCollectedEvent -= OnLootCollectedEvent;
    }
    
    private void OnLootCollectedEvent(int amount)
    {
        LootTotal -= amount;
    }

}
