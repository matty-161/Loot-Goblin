using Godot;
using LootGoblin.Autoloads;

namespace LootGoblin.Systems;

public partial class EntityManager : Node
{
    [Export] private Node2D _entityRoot;
    
    public override void _Ready()
    {
        GameplaySignalBus.Instance.LevelTransitionEvent += ClearEntities;
    }

    public override void _ExitTree()
    {
        GameplaySignalBus.Instance.LevelTransitionEvent -= ClearEntities;
    }

    private void ClearEntities()
    {
        foreach (Node node in _entityRoot.GetChildren())
        {
            node.QueueFree();
        }
    }
}