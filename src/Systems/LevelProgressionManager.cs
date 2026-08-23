using Godot;

namespace LootGoblin;

public partial class LevelProgressionManager : Node
{
    private int _currentLevel = 1;
    
    public int CurrentLevel 
    { 
        get => _currentLevel;
        private set
        {
            _currentLevel = value;
            if (_currentLevel > 3)
            {
                GameEventManager.FinalLevelCompleteEvent?.Invoke();
            }
            else
            {
                GameEventManager.NextLevelEvent?.Invoke();
            }
        }
    }

    public override void _Ready()
    {
        GameEventManager.LevelTransitionEvent += OnLevelTransition;
    }

    public override void _ExitTree()
    {
        GameEventManager.LevelTransitionEvent -= OnLevelTransition;
    }

    private void OnLevelTransition()
    {
        CurrentLevel += 1;
    }


}