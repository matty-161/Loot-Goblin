using Godot;

namespace LootGoblin;

public partial class LevelBossChase : LevelBossState
{
    public override string StateName { get; set; } = "LevelBossChase";

    [Export] private NavigationAgent2D _agent;

    private float _attackPixelRange;

    public override void _Ready()
    {
        _attackPixelRange = Stats.AttackRange * LevelTileMap.TileSize;
        base._Ready();
    }

    public override void PhysicsUpdate(double delta)
    {
        if (Motor.GlobalPosition.DistanceTo(PlayerData.PlayerPosition) < _attackPixelRange)
        {
            Finished?.Invoke(Attack);
            return;
        }

        ChasePlayer();
    }

    private void ChasePlayer()
    {
        _agent.SetTargetPosition(PlayerData.PlayerPosition);
        if (_agent.IsNavigationFinished())
        {
            return;
        }

        Vector2 nextPathDir = Motor.GlobalPosition.DirectionTo(_agent.GetNextPathPosition());
        Motor.MoveDirection = nextPathDir;
    }
}