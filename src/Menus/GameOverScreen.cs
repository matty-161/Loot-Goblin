using Godot;
using System;

namespace LootGoblin;

public partial class GameOverScreen : Control
{
    [Export] private LootManager _lootManager;
    
    private Button _mainMenuButton;
    private Label _lootLabel;

    public override void _Ready()
    {
        Hide();
        
        _mainMenuButton = GetNode<Button>("%ReturnToMenuButton");
        _lootLabel = GetNode<Label>("%LootLabel");

        _mainMenuButton.Pressed += OnMainMenuButtonPressed;

        GameEventManager.PlayerDied += OnPlayerDied;
    }

    private void OnPlayerDied()
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
