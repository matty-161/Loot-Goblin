using Godot;
using System;
using LootGoblin;

public partial class GameCompletionScreen : Control
{
	private Button _mainMenuButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
        
		_mainMenuButton = GetNode<Button>("%ReturnToMenuButton");
		_mainMenuButton.Pressed += OnMainMenuButtonPressed;

		GameEventManager.FinalLevelCompleteEvent += Show;
	}

	private void OnMainMenuButtonPressed()
	{
		GetTree().SetPause(false);
		GetTree().ReloadCurrentScene();
	}
}
