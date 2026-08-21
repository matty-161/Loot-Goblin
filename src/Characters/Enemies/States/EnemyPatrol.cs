using Godot;
using Godot.Collections;

namespace LootGoblin;

[GlobalClass]
public partial class EnemyPatrol : EnemyState
{
    [ExportCategory("Node References")]
    [Export] private NavigationAgent2D _agent;

    private float _patrolTargetBufferDistance;
    private float _chaseTriggerPixelDistance;
    
    private Array<Vector2I> _patrolTargets = [];
    private int _targetIndex;
    
    public override string StateName { get; set; } = "Patrol";

    public override void _Ready()
    {
        _patrolTargetBufferDistance = Stats.PatrolTargetBufferDistance * LevelTileMap.TileSize;
        _chaseTriggerPixelDistance = Stats.PatrolChaseTriggerDistance * LevelTileMap.TileSize;
        base._Ready();
    }

    public override void Enter(string previousStatePath)
    {
        _agent.SetTargetPosition(_patrolTargets[_targetIndex]);
    }

    public override void PhysicsUpdate(double delta)
    {
        // the agent can get stuck and repeatedly overshoot the target
        // so i use a buffer so it only has to get in a certain range of the target
        // if the enemy is in range of the target, enter idle state
        if (_agent.DistanceToTarget() < _patrolTargetBufferDistance)
        {
            NextDestination();
            Finished?.Invoke(Idle);
            return;
        }
        
        // if the enemy is within chase range of the player, enter chase state
        if (Motor.GlobalPosition.DistanceTo(PlayerData.PlayerPosition) < _chaseTriggerPixelDistance)
        {
            Finished?.Invoke(Chase);
            return;
        }

        if (_agent.IsNavigationFinished())
        {
            return;
        }

        Vector2 nextPathDir = Motor.GlobalPosition.DirectionTo(_agent.GetNextPathPosition());
        Motor.MoveDirection = nextPathDir;
    }

    private void NextDestination()
    {
        _targetIndex += 1;
        _targetIndex %= _patrolTargets.Count;
    }

    public void AddPatrolTarget(Vector2I target)
    {
        _patrolTargets.Add(target);
    }
}