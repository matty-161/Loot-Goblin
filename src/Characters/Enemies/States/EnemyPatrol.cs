using Godot;
using LootGoblin.LevelGeneration;

namespace LootGoblin.Characters.Enemies.States;

[GlobalClass]
public partial class EnemyPatrol : EnemyState
{
    [ExportCategory("Patrol Settings")]
    [Export] private float _distanceToPatrolTarget;
    [Export] private float _chaseTriggerDistance;
    
    private float PixelDistanceToPatrolTarget => _distanceToPatrolTarget * LevelTileMap.TileSize;
    private float ChaseTriggerPixelDistance => _chaseTriggerDistance * LevelTileMap.TileSize;

    [ExportCategory("Node References")]
    [Export] private NavigationAgent2D _agent;
    
    public override void Enter(string previousStatePath)
    {
        // _agent.SetTargetPosition();
    }
}