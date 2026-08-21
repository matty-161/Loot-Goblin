using System.Diagnostics;
using Godot;

namespace LootGoblin;

public abstract partial class EnemyState : State
{
    protected const string Idle = "EnemyIdle";
    protected const string Patrol = "EnemyPatrol";
    protected const string Chase = "EnemyChase";
    protected const string Attack = "EnemyAttack";

    protected CharacterMotor Motor;
    [Export] protected EnemyStats Stats;

    public override void _Ready()
    {
        // CallDeferred("SetMotor");
        Motor = GetOwner<CharacterMotor>();
    }

    private void SetMotor()
    {
        Motor = GetOwner<CharacterMotor>();
        Debug.Assert(Motor != null, "EnemyState must be used in a CharacterMotor scene");
    }

}