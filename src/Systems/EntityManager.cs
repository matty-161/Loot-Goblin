using Godot;

namespace LootGoblin;

public partial class EntityManager : Node
{
    [Export] private Node2D _entityRoot;
    
    public override void _Ready()
    {
        GameEventManager.LevelTransitionEvent += ClearEntities;
    }

    public override void _ExitTree()
    {
        GameEventManager.LevelTransitionEvent -= ClearEntities;
    }

    private void ClearEntities()
    {
        foreach (Node node in _entityRoot.GetChildren())
        {
            node.QueueFree();
        }
    }
}