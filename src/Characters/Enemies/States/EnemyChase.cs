using Godot;

namespace LootGoblin;

[GlobalClass]
public partial class EnemyChase : EnemyState
{
    [ExportCategory("Node References")]
    [Export] private NavigationAgent2D _agent;
 
    private float _chaseQuitPixelDistance;
    private float _attackPixelRange;
    
    public override string StateName { get; set; } = "Chase";

    public override void _Ready()
    {
        _chaseQuitPixelDistance = Stats.ChaseQuitDistance * LevelTileMap.TileSize;
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

        if (Motor.GlobalPosition.DistanceTo(PlayerData.PlayerPosition) < _chaseQuitPixelDistance)
        {
            ChasePlayer();
        }
        else
        {
            Finished?.Invoke(Patrol);
        }
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