using System.Diagnostics;
using LootGoblin.Globals;

namespace LootGoblin.Characters.Enemies.States;

public abstract partial class EnemyState : State
{
    protected const string Idle = "EnemyIdle";
    protected const string Patrol = "EnemyPatrol";
    protected const string Chase = "EnemyChase";
    protected const string Attack = "EnemyAttack";

    protected CharacterMotor Motor;

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