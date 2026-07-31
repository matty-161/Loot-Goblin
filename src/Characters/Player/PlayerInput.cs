using Godot;

namespace LootGoblin.Characters.Player;

public partial class PlayerInput : Node
{
    private CharacterMotor _characterMotor;

    public override void _Ready()
    {
        _characterMotor = GetParent<CharacterMotor>();
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 movementInput = Input.GetVector(
            "move_left", "move_right", 
            "move_up", "move_down");
        _characterMotor.MoveDirection = movementInput;
    }
}