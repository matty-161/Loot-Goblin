using Godot;
using System;

namespace LootGoblin;

public partial class GameManager : Node
{
    public static bool IsTest { get; set; } = false;
    [Export] public int FinalLevel;

    
    private float _bossRoomPixelRadius;
    private Node2D _levelBoss;
    
    public override void _Ready()
    {
        GameEventManager.PlayerDied += OnPlayerDied;
        GameEventManager.FinalLevelCompleteEvent += OnFinalLevelComplete;
        GameEventManager.LevelBossGenerated += OnLevelBossGenerated;
    }
    
    public override void _Process(double delta)
    {

        if (_levelBoss == null) return;
        if (_levelBoss.GlobalPosition.DistanceTo(PlayerData.PlayerPosition) <= _bossRoomPixelRadius)
        {
            GameEventManager.PlayerEnteredBossRoomEvent?.Invoke();
            _levelBoss = null;
        }
    }
    
    private void OnLevelBossGenerated(int radius, Node2D boss)
    {
        _bossRoomPixelRadius = radius * LevelTileMap.TileSize;
        _levelBoss = boss;
    }

    private void OnPlayerDied()
    {
        GameEventManager.GameEnded?.Invoke();
        GetTree().SetPause(true);
    }

    private void OnFinalLevelComplete()
    {
        GameEventManager.GameEnded?.Invoke();
        // GetTree().SetPause(true);
    }
    
    
}
