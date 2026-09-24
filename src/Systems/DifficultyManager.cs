using Godot;
using System;

namespace LootGoblin;

public partial class DifficultyManager : Node
{
    [Export] private LevelProgressionManager _levelProgressionManager;
    
    public double RunTime { get; set; }

    public bool TimerIsRunning { get; set; }

    public static double DifficultyScale = 1;

    public override void _Ready()
    {
        GameEventManager.GameStarted += OnGameStarted;
        GameEventManager.GameEnded += OnGameEnded;
    }

    public override void _ExitTree()
    {
        GameEventManager.GameStarted -= OnGameStarted;
        GameEventManager.GameEnded -= OnGameEnded;
    }

    public override void _Process(double delta)
    {
        if (TimerIsRunning)
        {
            RunTime += delta;
            DifficultyScale = (1 + Math.Round(RunTime / 60.0) * 0.4) * 
                              Math.Pow(1.1, _levelProgressionManager.CurrentLevel); // difficulty calculation
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
