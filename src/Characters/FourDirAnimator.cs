using Godot;

namespace LootGoblin;

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
        if (_characterMotor.MoveDirection != Vector2.Zero)
        {

            if (_characterMotor.MoveDirection.Normalized().Dot(Vector2.Right) > .707)
            {
                if (_previousAnimation == "walk_right") return;
                _sprite.Play("walk_right");
            }
            else if (_characterMotor.MoveDirection.Normalized().Dot(Vector2.Left) > .707)
            {
                if (_previousAnimation == "walk_left") return;
                _sprite.Play("walk_left");
            }
            else if (_characterMotor.MoveDirection.Normalized().Dot(Vector2.Up) > .707)
            {
                if (_previousAnimation == "walk_up") return;
                _sprite.Play("walk_up");
            }
            else
            {
                if (_previousAnimation == "walk_down") return;
                _sprite.Play("walk_down");
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

    public void PlayAttackAnimation()
    {
        Vector2 playerPos = PlayerData.PlayerPosition;
        Vector2 thisPos = _characterMotor.Position;

        if (thisPos.DirectionTo(playerPos).Dot(Vector2.Right) > .707)
        {
            if (_previousAnimation == "attack_right") return;
            _sprite.Play("attack_right");
        }
        else if (thisPos.DirectionTo(playerPos).Dot(Vector2.Left) > .707)
        {
            if (_previousAnimation == "attack_left") return;
            _sprite.Play("attack_left");
        }
        else if (thisPos.DirectionTo(playerPos).Dot(Vector2.Up) > .707)
        {
            if (_previousAnimation == "attack_up") return;
            _sprite.Play("attack_up");
        }
        else
        {
            if (_previousAnimation == "attack_down") return;
            _sprite.Play("attack_down");
        }
        
        // old attack animation version
        // switch (_previousAnimation)
        // {
        //     case "walk_right":
        //         _sprite.Play("attack_right");
        //         break;
        //     case "walk_left":
        //         _sprite.Play("attack_left");
        //         break;
        //     case "walk_down":
        //         _sprite.Play("attack_down");
        //         break;
        //     case "walk_up":
        //         _sprite.Play("attack_up");
        //         break;
        // }
    }
}