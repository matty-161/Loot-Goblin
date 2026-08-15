using Godot;

namespace LootGoblin;

[GlobalClass]
public partial class EnemyIdle : EnemyState
{
    [ExportCategory("Node References")]
    [Export] private FourDirAnimator _animator;
    
    private float _chaseTriggerPixelDistance;
    private Timer _timer;
    
    public override string StateName { get; set; } = "Idle";

    public override void _Ready()
    {
        _chaseTriggerPixelDistance = Stats.IdleChaseTriggerDistance * LevelTileMap.TileSize;
        base._Ready();
    }

    public override void Enter(string previousStatePath)
    {
        Motor.MoveDirection = Vector2.Zero;
        SetupTimer();
    }

    public override void PhysicsUpdate(double delta)
    {
        if (Motor.GlobalPosition.DistanceTo(PlayerData.PlayerPosition) < _chaseTriggerPixelDistance)
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
        _timer.WaitTime = Stats.IdleWaitTime;
        _timer.Start();
    }
    
}