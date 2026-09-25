using System;
using Godot;

namespace LootGoblin;

[GlobalClass]
public partial class HealthComponent : Node
{
    public Action TookDamage;
    
    [Export] private StatsComponent _statsComponent;
    
    public Action<int> HealthChangedEvent;
    public Action DiedEvent;
    
    public int MaxHealth { get; set; }

    private int _health;
    
    public int Health
    {
        get => _health;
        private set
        {
            if (value > MaxHealth)
            {
                HealthChangedEvent?.Invoke(MaxHealth);
                _health = MaxHealth;
            }
            else
            {
                HealthChangedEvent?.Invoke(value);
                _health = value;
            }
        }
    }
	
    [Export] public Hurtbox2D Hurtbox {get; private set;}
	
    public override void _Ready()
    {
        Hurtbox.TakeDamageEvent += TakeDamage;
        DiedEvent += OnDeath;
        
        MaxHealth = _statsComponent.MaxHealth;
        
        Health = MaxHealth;
        if (Owner.IsInGroup("player"))
        {
            PlayerData.MaxHealth = MaxHealth;
        }
    }

    public override void _ExitTree()
    {
        Hurtbox.TakeDamageEvent -= TakeDamage;
    }

    private void TakeDamage(int amount)
    {
        TookDamage?.Invoke();
        Health -= amount;
        if (Health <= 0) DiedEvent?.Invoke();
    }

    public void Heal(int amount)
    {
        Health += amount;
    }

    private void OnDeath()
    {
        Owner.QueueFree();
    }
}