using Godot;
using System;
using LootGoblin.LevelGeneration;

namespace LootGoblin.Menus;

public partial class MainMenu : Control
{
	[Export] private LevelGenerationManager _levelGenerationManager;

	private Button _playButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playButton = GetNode<Button>("%PlayButton");
		_playButton.Pressed += StartGame;
	}

	private void StartGame()
	{
		GD.Print("Game started");
		_levelGenerationManager.GenerateLevel();
		Hide();
	}
}
