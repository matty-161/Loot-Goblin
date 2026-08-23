using Godot;
using System;

namespace LootGoblin;

public partial class GameOverScreen : Control
{
    private Button _mainMenuButton;

    public override void _Ready()
    {
        Hide();
        
        _mainMenuButton = GetNode<Button>("%ReturnToMenuButton");

        _mainMenuButton.Pressed += OnMainMenuButtonPressed;

        GameEventManager.PlayerDied += Show;
    }

    private void OnMainMenuButtonPressed()
    {
        GetTree().SetPause(false);
        GetTree().ReloadCurrentScene();
    }
}
