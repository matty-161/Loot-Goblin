using Godot;
using Godot.Collections;

namespace LootGoblin;

[GlobalClass]
public partial class LevelBossAttack: LevelBossState
{
    public override string StateName { get; set; } = "LevelBossAttack";
    
    [ExportCategory("Node References")]
    [Export] private FourDirAnimator _animator;
    [Export] private Hitbox2D _hitbox;
    [Export] private AnimatedSprite2D _animatedSprite;
    
    // locations should be in order: top, right, bottom, left
    [Export] private Array<Node2D> _hitboxLocations;

    private bool _isAttacking;

    public bool IsAttacking
    {
        get => _isAttacking;
        set
        {
            if (value)
            {
                _hitbox.Monitorable = true;
                _hitbox.Monitoring = true;
            }
            else
            {
                _hitbox.Monitorable = false;
                _hitbox.Monitoring = false;
            }
            _isAttacking = value;
        }
    }

    private bool _canAttack;

    private float _attackDisengagePixelRange;

    public override void _Ready()
    {
        _attackDisengagePixelRange = Stats.AttackRange * LevelTileMap.TileSize;
        _hitbox.Damage = Stats.Damage;
        
        base._Ready();
    }

    public override void Enter(string previousStatePath)
    {
        _canAttack = true;
        Motor.MoveDirection = Vector2.Zero;
        _animatedSprite.FrameChanged += OnFrameChanged;
    }

    public override void Exit()
    {
        IsAttacking = false;
        _animatedSprite.FrameChanged -= OnFrameChanged;
    }

    public override void PhysicsUpdate(double delta)
    {
        if (Motor.GlobalPosition.DistanceTo(PlayerData.PlayerPosition) > _attackDisengagePixelRange)
        {
            Finished?.Invoke(Chase);
        }

        if (!_canAttack) return;

        AttackPlayer();
    }

    private void AttackPlayer()
    {
        _hitbox.CanDamage = true;
        
        _animator.PlayAttackAnimation();

        switch (_animatedSprite.Animation)
        {
            case "attack_up":
                _hitbox.Position = _hitboxLocations[0].Position;
                break;
            case "attack_right":
                _hitbox.Position = _hitboxLocations[1].Position;
                break;
            case "attack_down":
                _hitbox.Position = _hitboxLocations[2].Position;
                break;
            case "attack_left":
                _hitbox.Position = _hitboxLocations[3].Position;
                break;
        }
        
        _canAttack = false;
        
        Timer timer = new();
        AddChild(timer);
        timer.WaitTime = Stats.AttackCooldown;
        timer.Start();
        timer.Timeout += () =>
        {
            _canAttack = true;
            timer.QueueFree();
        };
    }

    private void OnFrameChanged()
    {
        if (_animatedSprite.Animation != "attack_up" &&
            _animatedSprite.Animation != "attack_left" &&
            _animatedSprite.Animation != "attack_down" &&
            _animatedSprite.Animation != "attack_right")
        {
            return;
        }
        
        if (_animatedSprite.Frame == 3)
        {
            IsAttacking = true;
        }
        else if (_animatedSprite.Frame == 4)
        {
            IsAttacking = false;
        }
    }
}