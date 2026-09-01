using Godot;
using System;

namespace LootGoblin;

[GlobalClass]
public partial class LevelBossIdle : LevelBossState
{
    public override string StateName { get; set; } = "LevelBossIdle";

    public override void _Ready()
    {
        GameEventManager.PlayerEnteredBossRoomEvent += OnPlayerEnteredBossRoom;
    }

    private void OnPlayerEnteredBossRoom()
    {
        Finished?.Invoke(Chase);
    }
    
}
