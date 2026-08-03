using Godot;

namespace LootGoblin;

[GlobalClass]
public partial class EnemyIdle : EnemyState
{
    [ExportCategory("Idle Settings")]
    [Export] private float _idleWaitTime;
    [Export] private float _chaseTriggerDistance;

    private float ChaseTriggerPixelDistance => _chaseTriggerDistance * LevelTileMap.TileSize;

    [ExportCategory("Node References")]
    [Export] private FourDirAnimator _animator;

    private Timer _timer;

    public override void Enter(string previousStatePath)
    {
        Motor.MoveDirection = Vector2.Zero;
        SetupTimer();
    }

    public override void PhysicsUpdate(double delta)
    {
        if (Motor.GlobalPosition.DistanceTo(PlayerData.PlayerPosition) < ChaseTriggerPixelDistance)
        {
            Finished?.Invoke(Chase);
        }
    }

    private void IdleTimerTimeout()
    {
        Finished?.Invoke(Patrol);
    }

    private void SetupTimer()
    {
        _timer = new();
        _timer.OneShot = true;
        AddChild(_timer);
        _timer.Timeout += IdleTimerTimeout;
        _timer.WaitTime = _idleWaitTime;
        _timer.Start();
    }
    
}