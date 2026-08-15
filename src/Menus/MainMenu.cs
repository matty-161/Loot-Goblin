using Godot;
using System;

namespace LootGoblin;

public partial class MainMenu : Control
{
	[Export] private LevelGenerationManager _levelGenerationManager;

	private Control _mainMenuContainer;
	private SettingsContainer _settingsContainer;

	private Button _playButton;
	private Button _settingsButton;
	private Button _quitButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainMenuContainer = GetNode<Control>("%MainMenuContainer");
		_settingsContainer = GetNode<SettingsContainer>("%SettingsContainer");
		
		_mainMenuContainer.Show();
		_settingsContainer.Hide();

		_settingsContainer.BackButtonPressed += OnSettingsBackButtonPressed;
		
		_playButton = GetNode<Button>("%PlayButton");
		_settingsButton = GetNode<Button>("%SettingsButton");
		_quitButton = GetNode<Button>("%QuitButton");
		
		_playButton.Pressed += StartGame;
		_settingsButton.Pressed += OpenSettings;
		_quitButton.Pressed += QuitGame;
	}

	public override void _ExitTree()
	{
		_playButton.Pressed -= StartGame;
	}

	private void StartGame()
	{
		GD.Print("Game started");
		_levelGenerationManager.GenerateLevel(false);
		Hide();
	}

	private void QuitGame()
	{
		GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
		GetTree().Quit();
	}

	private void OpenSettings()
	{
		_mainMenuContainer.Hide();
		_settingsContainer.Show();
	}

	private void OnSettingsBackButtonPressed()
	{
		_mainMenuContainer.Show();
		_settingsContainer.Hide();
	}
}
