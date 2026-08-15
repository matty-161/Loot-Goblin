using System;
using Godot;

namespace LootGoblin;

[GlobalClass]
public partial class HealthComponent : Node
{
    public Action<int> HealthChangedEvent;

    private int _health;
    
    [Export]
    public int Health
    {
        get => _health;
        private set
        {
            HealthChangedEvent?.Invoke(value);
            _health = value;
        }
    }
	
    [Export] public Hurtbox2D Hurtbox {get; private set;}
	
    public override void _Ready()
    {
        Hurtbox.TakeDamageEvent += TakeDamage;
    }

    public override void _ExitTree()
    {
        Hurtbox.TakeDamageEvent -= TakeDamage;
    }

    private void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0) OnDeath();
    }

    private void OnDeath()
    {
        Owner.QueueFree();
    }
}