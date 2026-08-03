using System.Threading.Tasks;
using Godot;

namespace LootGoblin;

[GlobalClass]
public partial class EnemyAttack : EnemyState
{
    [Export] private int _damage;
    [Export] private int _attackRange;
    [Export] private int _attackCooldown;
    
    [ExportCategory("Node References")]
    [Export] private FourDirAnimator _animator;

    private bool _isAttacking = false;

    public override void PhysicsUpdate(double delta)
    {
        if (_isAttacking) return;

        if (Motor.GlobalPosition.DistanceTo(PlayerData.PlayerPosition) > _attackRange)
        {
            Finished?.Invoke(Chase);
        }
        else
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        // TODO add attack animations
        // _animator.PlayAttackAnimation();

        _isAttacking = true;
        Timer timer = new();
        AddChild(timer);
        timer.WaitTime = _attackCooldown;
        timer.Start();
        timer.Timeout += () =>
        {
            _isAttacking = false;
            timer.QueueFree();
        };
    }
}