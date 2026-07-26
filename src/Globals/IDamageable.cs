using System;

namespace LootGoblin.Globals;

public interface IDamageable
{
    event Action<IDamageable> DeathEvent;
    
    int Health { get; }
    Hurtbox2D Hurtbox { get; }
    
    void TakeDamage(int amount);
    void OnDeath();
}