using Godot;
using LootGoblin.Autoloads;

namespace LootGoblin.Characters.Enemies;

public partial class BossComponent : Node
{
    public override void _ExitTree()
    {
        GameplaySignalBus.Instance.LevelBossDiedEvent?.Invoke();
    }
}