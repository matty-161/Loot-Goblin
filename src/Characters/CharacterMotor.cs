using Godot;
using LootGoblin.LevelGeneration;

namespace LootGoblin;

public partial class CharacterMotor : CharacterBody2D
{
    [Export] private float _speed = 10f;

    private float SpeedInPixels => _speed * LevelTileMap.TileSize;

    public Vector2 MoveDirection;

    public override void _PhysicsProcess(double delta)
    {
        Velocity = SpeedInPixels * MoveDirection;
        MoveAndSlide();
    }
}