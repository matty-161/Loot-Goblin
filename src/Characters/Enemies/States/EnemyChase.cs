using Godot;
using LootGoblin.Characters.Player;
using LootGoblin.LevelGeneration;

namespace LootGoblin.Characters.Enemies.States;

[GlobalClass]
public partial class EnemyChase : EnemyState
{
    [ExportCategory("Chase Settings")]
    [Export] private float _chaseQuitDistance;
    [Export] private float _attackRange;

    private float ChaseQuitPixelDistance => _chaseQuitDistance * LevelTileMap.TileSize;
    private float AttackPixelRange => _attackRange *  LevelTileMap.TileSize;
    
    [ExportCategory("Node References")]
    [Export] private NavigationAgent2D _agent;
    
    
    public override void PhysicsUpdate(double delta)
    {
        if (Motor.GlobalPosition.DistanceTo(PlayerData.PlayerPosition) < AttackPixelRange)
        {
            Finished?.Invoke(Attack);
            return;
        }

        if (Motor.GlobalPosition.DistanceTo(PlayerData.PlayerPosition) < ChaseQuitPixelDistance)
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