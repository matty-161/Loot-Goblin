using Godot;
using System;

namespace LootGoblin;

public partial class GameManager : Node
{
    public static bool IsTest { get; set; } = false;
    [Export] public int FinalLevel;

    public override void _Ready()
    {
        GameEventManager.PlayerDied += OnPlayerDied;
        GameEventManager.FinalLevelCompleteEvent += OnFinalLevelComplete;
    }

    private void OnPlayerDied()
    {
        GetTree().SetPause(true);
    }

    private void OnFinalLevelComplete()
    {
        // GetTree().SetPause(true);
    }
}
