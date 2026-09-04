using Godot;
using System;

namespace LootGoblin;

public partial class DifficultyManager : Node
{
    public double RunTime { get; set; }

    public bool TimerIsRunning { get; set; }

    public override void _Ready()
    {
        GameEventManager.GameStarted += OnGameStarted;
        GameEventManager.GameEnded += OnGameEnded;
    }

    public override void _Process(double delta)
    {
        if (TimerIsRunning)
        {
            RunTime += delta;
        }
    }
    
    private void OnGameStarted()
    {
        TimerIsRunning = true;
    }
    
    private void OnGameEnded()
    {
        TimerIsRunning = false;
        ResetTimer();
    }

    private void ResetTimer()
    {
        RunTime = 0.0;
    }
}
