using Godot;
using LootGoblin.Autoloads;
using LootGoblin.Player;

namespace LootGoblin.LevelGeneration.LevelContents;

public partial class LevelExit : Area2D
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
		BodyEntered += OnAreaEntered;
		GameplaySignalBus.Instance.LevelBossDiedEvent += OnLevelBossDiedEvent;
	}

	public override void _ExitTree()
	{
		GameplaySignalBus.Instance.LevelBossDiedEvent -= OnLevelBossDiedEvent;
	}

	private void OnAreaEntered(Node2D body)
	{
		if (body is PlayerController)
		{
			GameplaySignalBus.Instance.LevelTransitionFadeOutEvent?.Invoke();
		}
	}

	private void OnLevelBossDiedEvent()
	{
		Show();
	}
}