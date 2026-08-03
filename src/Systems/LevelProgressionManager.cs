using Godot;

namespace LootGoblin;

public partial class LevelProgressionManager : Node
{
    public int CurrentLevel { get; set; } = 1;

    public override void _Ready()
    {
        GameplaySignalBus.Instance.LevelTransitionEvent += OnLevelTransition;
    }

    public override void _ExitTree()
    {
        GameplaySignalBus.Instance.LevelTransitionEvent -= OnLevelTransition;
    }

    private void OnLevelTransition()
    {
        CurrentLevel += 1;
    }
}