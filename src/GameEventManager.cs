using System;

namespace LootGoblin;

public static class GameEventManager
{
    // loot events
    public static Action<int> LootChangedEvent;
    public static Action<int> LootCollectedEvent;
    
    // level events
    public static Action PlayerEnteredBossRoomEvent;
    
    public static Action LevelBossDiedEvent;
    public static Action LevelTransitionFadeOutEvent;
    public static Action LevelTransitionEvent;
    public static Action<bool> LevelTransitionFadeInEvent;

    public static Action NextLevelEvent;
    
    public static Action FinalLevelCompleteEvent;
    
    // player events
    public static Action<int> PlayerHealthChangedEvent;
    public static Action PlayerDied;
}