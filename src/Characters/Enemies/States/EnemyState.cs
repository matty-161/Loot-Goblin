using System.Diagnostics;
using Godot;

namespace LootGoblin;

public abstract partial class EnemyState : State, IEnemyState
{
    protected const string Idle = "EnemyIdle";
    protected const string Patrol = "EnemyPatrol";
    protected const string Chase = "EnemyChase";
    protected const string Attack = "EnemyAttack";

    public CharacterMotor Motor { get; private set; }
    [Export] public EnemyStats Stats { get; set; }

    public override void _Ready()
    {
        Motor = GetOwner<CharacterMotor>();
    }
}