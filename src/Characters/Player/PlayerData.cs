using Godot;

namespace LootGoblin.Characters.Player;

public partial class PlayerData : Node
{
    private CharacterMotor _playerMotor;

    public static Vector2 PlayerPosition;

    public override void _Ready()
    {
        _playerMotor = GetParent<CharacterMotor>();
    }

    public override void _PhysicsProcess(double delta)
    {
        PlayerPosition = _playerMotor.GlobalPosition;
    }
}