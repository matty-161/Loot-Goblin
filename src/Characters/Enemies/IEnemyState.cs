namespace LootGoblin;

public interface IEnemyState
{
    CharacterMotor Motor { get; }
    EnemyStatsComponent Stats { get; }
}