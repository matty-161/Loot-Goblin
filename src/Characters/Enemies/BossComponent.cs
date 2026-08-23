using Godot;

namespace LootGoblin;

public partial class BossComponent : Node
{
    public override void _ExitTree()
    {
        GameEventManager.LevelBossDiedEvent?.Invoke();
    }
}