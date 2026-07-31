using Godot;

namespace LootGoblin.Characters;

public partial class FourDirAnimator : Node
{
    private CharacterMotor _characterMotor;
    
    [Export] private AnimatedSprite2D _sprite;

    private string _previousAnimation;

    public override void _Ready()
    {
        _characterMotor = GetParent<CharacterMotor>();
    }
    
    public override void _PhysicsProcess(double delta)
    {
        _previousAnimation  = _sprite.Animation;
        if (_characterMotor.Velocity != Vector2.Zero)
        {
            switch (_characterMotor.Velocity.X)
            {
                case > 0:
                    _sprite.Play("walk_right");
                    break;
                case < 0:
                    _sprite.Play("walk_left");
                    break;
            }

            switch (_characterMotor.Velocity.Y)
            {
                case > 0:
                    _sprite.Play("walk_down");
                    break;
                case < 0:
                    _sprite.Play("walk_up");
                    break;
            }
        }
        else
        {
            switch (_previousAnimation)
            {
                case "walk_right":
                    _sprite.Play("idle_right");
                    break;
                case "walk_left":
                    _sprite.Play("idle_left");
                    break;
                case "walk_down":
                    _sprite.Play("idle_down");
                    break;
                case "walk_up":
                    _sprite.Play("idle_up");
                    break;
            }
        }
        
    }
}