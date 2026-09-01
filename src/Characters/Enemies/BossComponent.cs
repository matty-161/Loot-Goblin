using Godot;

namespace LootGoblin;

[GlobalClass]
public partial class BossComponent : Node
{
    public override void _ExitTree()
    {
        GameEventManager.LevelBossDiedEvent?.Invoke();
    }
}