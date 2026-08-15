using Godot;
using System;

namespace LootGoblin;

public partial class SettingsContainer : VBoxContainer
{
	public Action BackButtonPressed;
	
	private CheckButton _fullscreenToggle;
	private Button _backButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_fullscreenToggle = GetNode<CheckButton>("%FullscreenToggle");
		_backButton = GetNode<Button>("%BackButton");
		
		_fullscreenToggle.Toggled += OnFullscreenToggle;
		_backButton.Pressed += () => BackButtonPressed?.Invoke();
	}

	private void OnFullscreenToggle(bool isToggled)
	{
		if (isToggled)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
		else
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}
	}
}
