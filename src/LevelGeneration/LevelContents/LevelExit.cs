using Godot;
using LootGoblin.Autoloads;

namespace LootGoblin.LevelGeneration.LevelContents;

public partial class LevelExit : Area2D
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
		AreaEntered += OnAreaEntered;
		GameplaySignalBus.Instance.LevelBossDiedEvent += OnLevelBossDiedEvent;
	}

	public override void _ExitTree()
	{
		GameplaySignalBus.Instance.LevelBossDiedEvent -= OnLevelBossDiedEvent;
	}

	private void OnAreaEntered(Area2D area)
	{
		GameplaySignalBus.Instance.LevelTransitionFadeOutEvent?.Invoke();
	}

	private void OnLevelBossDiedEvent()
	{
		Show();
	}
}