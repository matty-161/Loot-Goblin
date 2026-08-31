namespace LootGoblin;

public interface IEnemyState
{
    CharacterMotor Motor { get; }
    EnemyStats Stats { get; }
}