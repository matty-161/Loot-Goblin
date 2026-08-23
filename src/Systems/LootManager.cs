using Godot;
using System;

namespace LootGoblin;

public partial class LootManager : Node
{
    private int _lootTotal = 0;

    public int LootTotal
    {
        get => _lootTotal;
        set
        {
            _lootTotal = value;
            GameEventManager.LootChangedEvent?.Invoke(_lootTotal);
        }
    }

    public override void _Ready()
    {
        GameEventManager.LootCollectedEvent += OnLootCollectedEvent;
        GameEventManager.FinalLevelCompleteEvent += OnFinalLevelComplete;
    }

    public override void _ExitTree()
    {
        GameEventManager.LootCollectedEvent -= OnLootCollectedEvent;
    }
    
    private void OnLootCollectedEvent(int amount)
    {
        LootTotal += amount;
    }

    private void OnFinalLevelComplete()
    {
        SaveManager.Instance.SaveScore(LootTotal);
    }

}
