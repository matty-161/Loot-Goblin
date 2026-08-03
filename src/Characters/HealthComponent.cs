using Godot;

namespace LootGoblin;

[GlobalClass]
public partial class HealthComponent : Node
{
    [Export] public int Health { get; private set; }
	
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