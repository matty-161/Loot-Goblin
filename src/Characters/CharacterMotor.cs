using Godot;
using LootGoblin;

namespace LootGoblin;

public partial class CharacterMotor : CharacterBody2D
{
    [Export] private StatsComponent _statsComponent;
    
    private float _speedInPixels;

    public Vector2 MoveDirection;

    public override void _Ready()
    {
        _speedInPixels = _statsComponent.MoveSpeed * LevelTileMap.TileSize;
    }

    public override void _PhysicsProcess(double delta)
    {
        
        Velocity = _speedInPixels * MoveDirection;
        MoveAndSlide();
    }
}