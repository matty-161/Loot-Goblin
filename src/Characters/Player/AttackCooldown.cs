using Godot;
using System;


namespace LootGoblin;

public partial class AttackCooldown : Timer
{
    [Export] private PlayerStatsComponent _statsComponent;

    public override void _Ready()
    {
        WaitTime = _statsComponent.AttackCooldown;
    }
    
}
