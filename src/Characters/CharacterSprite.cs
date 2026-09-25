using Godot;
using System;

namespace LootGoblin;
[GlobalClass]
public partial class CharacterSprite : AnimatedSprite2D
{
	[Export] private HealthComponent _healthComponent;
	[Export] private AnimationPlayer _animationPlayer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_healthComponent.TookDamage += OnTookDamage;
	}

	private void OnTookDamage()
	{
		_animationPlayer.Play("hitflash");
	}
}
