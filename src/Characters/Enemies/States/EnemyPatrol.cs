using Godot;
using Godot.Collections;

namespace LootGoblin;

[GlobalClass]
public partial class EnemyPatrol : EnemyState
{
    [ExportCategory("Patrol Settings")]
    [Export] private float _patrolTargetBufferDistance;
    [Export] private float _chaseTriggerDistance;
    
    private float PatrolTargetBufferDistance => _patrolTargetBufferDistance * LevelTileMap.TileSize;
    private float ChaseTriggerPixelDistance => _chaseTriggerDistance * LevelTileMap.TileSize;

    [ExportCategory("Node References")]
    [Export] private NavigationAgent2D _agent;

    private Array<Vector2I> _patrolTargets = [];
    private int _targetIndex;
    
    public override void Enter(string previousStatePath)
    {
        _agent.SetTargetPosition(_patrolTargets[_targetIndex]);
    }

    public override void PhysicsUpdate(double delta)
    {
        // the agent can get stuck and repeatedly overshoot the target
        // so we use a buffer so it only has to get in a certain range of the target
        // if the enemy is in range of the target, enter idle state
        if (_agent.DistanceToTarget() < PatrolTargetBufferDistance)
        {
            NextDestination();
            Finished?.Invoke(Idle);
            return;
        }
        
        // if the enemy is within chase range of the player, enter chase state
        if (Motor.GlobalPosition.DistanceTo(PlayerData.PlayerPosition) < ChaseTriggerPixelDistance)
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