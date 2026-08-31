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

	private Label _highScoreLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Show();
		_mainMenuContainer = GetNode<Control>("%MainMenuContainer");
		_settingsContainer = GetNode<SettingsContainer>("%SettingsContainer");
		
		_mainMenuContainer.Show();
		_settingsContainer.Hide();

		_settingsContainer.BackButtonPressed += OnSettingsBackButtonPressed;
		
		_playButton = GetNode<Button>("%PlayButton");
		_settingsButton = GetNode<Button>("%SettingsButton");
		_quitButton = GetNode<Button>("%QuitButton");

		_highScoreLabel = GetNode<Label>("%HighScore");
		// SetDeferred(_highScoreLabel.Text, SaveManager.Instance.GetHighScore().ToString());
		_highScoreLabel.Text = "High Score: " + SaveManager.Instance.GetHighScore().ToString();
		
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
		_levelGenerationManager.GenerateLevel();
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
