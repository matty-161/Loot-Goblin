using Godot;
using System;

namespace LootGoblin;

public partial class GameManager : Node
{
    public static bool IsTest = false;

    public override void _Ready()
    {
        GameplaySignalBus.Instance.PlayerDied += OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        GetTree().SetPause(true);
    }
}
