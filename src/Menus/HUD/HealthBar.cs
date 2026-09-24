using Godot;
using System;

namespace LootGoblin;
public partial class HealthBar : CenterContainer
{
	[Export] private ProgressBar _progressBar;
	[Export] private Label _label;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameEventManager.PlayerHealthChangedEvent += OnPlayerHealthChanged;
		GameEventManager.PlayerMaxHealthSetEvent += SetValues;
	}

	public override void _ExitTree()
	{
		GameEventManager.PlayerHealthChangedEvent -= OnPlayerHealthChanged;
		GameEventManager.PlayerMaxHealthSetEvent -= SetValues;
	}

	private void SetValues(int maxHealth)
	{
		_progressBar.MaxValue = maxHealth;
		_progressBar.Value = maxHealth;
		_label.Text = maxHealth + "/" + maxHealth;
	}

	private void OnPlayerHealthChanged(int newHealth)
	{
		_progressBar.Value = newHealth;
		_label.Text = newHealth + "/" + PlayerData.MaxHealth;
	}
}
