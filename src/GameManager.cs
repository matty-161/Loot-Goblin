using Godot;
using System;

namespace LootGoblin;

public partial class GameManager : Node
{
    public static bool IsTest { get; set; } = false;
    [Export] public int FinalLevel;

    [Export] private Node2D EntitiesRoot;
    [Export] private PackedScene _healthDropScn;

    
    private float _bossRoomPixelRadius;
    private Node2D _levelBoss;
    
    public override void _Ready()
    {
        GameEventManager.PlayerDied += OnPlayerDied;
        GameEventManager.FinalLevelCompleteEvent += OnFinalLevelComplete;
        GameEventManager.LevelBossGenerated += OnLevelBossGenerated;
        GameEventManager.DropHealthEvent += OnDropHealth;
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

    private void OnDropHealth(Vector2 position)
    {
        CallDeferred("DropHealth", position);
    }
    
    private void DropHealth(Vector2 position)
    {
        HealthDrop healthDrop = _healthDropScn.Instantiate<HealthDrop>();
        EntitiesRoot.AddChild(healthDrop);
        healthDrop.Position = position;
    }
    
    
}
