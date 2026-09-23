using Godot;

namespace LootGoblin;

public abstract partial class LevelBossState : State, IEnemyState
{
    protected const string Idle = "LevelBossIdle";
    protected const string Chase = "LevelBossChase";
    protected const string Attack = "LevelBossAttack";
    
    public CharacterMotor Motor { get; private set; }
    [Export] public EnemyStatsComponent Stats { get; set; }

    public override void _Ready()
    {
        Motor = GetOwner<CharacterMotor>();
    }
}