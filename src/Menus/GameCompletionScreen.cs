using Godot;
using System;
using LootGoblin;

public partial class GameCompletionScreen : Control
{
	[Export] private LootManager _lootManager;
	
	private Button _mainMenuButton;
	private Label _lootLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
        
		_mainMenuButton = GetNode<Button>("%ReturnToMenuButton");
		_lootLabel = GetNode<Label>("%LootLabel");
		
		_mainMenuButton.Pressed += OnMainMenuButtonPressed;

		GameEventManager.FinalLevelCompleteEvent += OnFinalLevelCompleted;
	}

	private void OnFinalLevelCompleted()
	{
		_lootLabel.Text = _lootManager.LootTotal.ToString();
		Show();
	}

	private void OnMainMenuButtonPressed()
	{
		GetTree().SetPause(false);
		GetTree().ReloadCurrentScene();
	}
}
