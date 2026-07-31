using Godot;

namespace LootGoblin.Characters;

public partial class TwoDirAnimator : Node
{
    private CharacterMotor _characterMotor;
    
    [Export] private AnimatedSprite2D _sprite;

    public override void _Ready()
    {
        _characterMotor = GetParent<CharacterMotor>();
    }
    
    public override void _PhysicsProcess(double delta)
    {
        if (_characterMotor.Velocity != Vector2.Zero)
        {
            _sprite.FlipH = _characterMotor.Velocity.X < 0;
            _sprite.Play("walk");
        }
        else _sprite.Play("idle");
    }
    
}