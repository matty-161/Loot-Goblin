using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace LootGoblin;

public partial class InteriorGenerator : Node
{
    
    private RoomGenerator _roomGenerator;
    private CorridorGenerator _corridorGenerator;

    [ExportCategory("Spawn Settings")] 
    [Export] private int _maxEnemiesPerRoom = 4;
    
    [ExportCategory("Spawnable Scene References")]
    [Export] private PackedScene _playerScene;
    [Export] private PackedScene _bossScene;
    [Export] private PackedScene _levelExitScene;
    [Export] private PackedScene _coinScene;
    [Export] private PackedScene _knightEnemyScene;

    [Export] private PackedScene _navigabilityEvaluator;
    [Export] private PackedScene _debugPatrolTarget;
    
    
    private Node2D _entitiesRoot;

    public void GenerateInteriors(
        Node2D entitiesRoot,
        RoomGenerator rg,
        CorridorGenerator cg)
    {
        _roomGenerator = rg;
        _corridorGenerator = cg;
        _entitiesRoot = entitiesRoot;
        Clear();
        SelectRooms();
        PlaceRoomStuff();
    }

    private void Clear()
    {
        foreach (Node entity in _entitiesRoot.GetChildren())
        {
            entity.QueueFree();
        }
    }

    private void SelectRooms()
    {
        List<Room> rooms = _roomGenerator.GetRooms();
        List<Room> roomsCopy = new List<Room>(rooms);
        
        // spawn room
        // randomly choose room to be spawn room, then remove from list so it can't be overridden
        int spawnIndex = (int)(GD.Randi() % roomsCopy.Count);
        roomsCopy[spawnIndex].Type = Room.RoomType.Spawn;
        roomsCopy.RemoveAt(spawnIndex);
        
        // boss room
        // choose room furthest from the spawn room to be the boss room
        Vector2I spawnRoomPosition = (rooms[spawnIndex].PositionTopLeft + rooms[spawnIndex].PositionBotRight) / 2;
        int furthestRoomIndex = 0;
        int furthestDistance = 0;
        for (int i = 0; i < roomsCopy.Count; i++)
        {
            Vector2I roomPos = (roomsCopy[i].PositionTopLeft + roomsCopy[i].PositionBotRight) / 2;
            if (roomPos.DistanceTo(spawnRoomPosition) > furthestDistance)
            {
                furthestDistance = (int)roomPos.DistanceTo(spawnRoomPosition);
                furthestRoomIndex = i;
            }
        }

        int bossIndex = furthestRoomIndex;
        roomsCopy[bossIndex].Type = Room.RoomType.Boss;
        roomsCopy.RemoveAt(bossIndex);
        
        // randomly choose room to be boss room, then remove from list so it can't be overridden
        // int bossIndex = (int)(GD.Randi() % roomsCopy.Count);
        // roomsCopy[bossIndex].Type = Room.RoomType.Boss;
        // roomsCopy.RemoveAt(bossIndex);
        
        // treasure rooms
        // make the rest of the rooms treasure rooms
        foreach (Room room in roomsCopy)
        {
            room.Type = Room.RoomType.Treasure;
        }
    }

    private void PlaceRoomStuff()
    {
        List<Room> rooms = _roomGenerator.GetRooms();

        // if this is a test, give the tester the list of rooms
        if (GameManager.IsTest) 
        {
            LevelGenerationTest.Rooms = rooms;
        }
        
        foreach (Room room in rooms)
        {
            
            switch (room.Type)
            {
                
                case Room.RoomType.Spawn:
                    CharacterBody2D player = _playerScene.Instantiate<CharacterBody2D>();
                    _entitiesRoot.AddChild(player);
                    player.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;

                    // if this is a test: add a navigability evaluator and give tester a reference to it
                    if (GameManager.IsTest)
                    {
                        RoomNavigabilityEvaluator evaluator = _navigabilityEvaluator.Instantiate<RoomNavigabilityEvaluator>();
                        _entitiesRoot.AddChild(evaluator);
                        evaluator.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;
                        LevelGenerationTest.NavigabilityEvaluator = evaluator;
                    }
                    
                    break;
                case Room.RoomType.Boss:
                    CharacterBody2D boss = _bossScene.Instantiate<CharacterBody2D>();
                    _entitiesRoot.AddChild(boss);
                    boss.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;

                    // send room radius and boss reference to GameManager
                    Vector2I roomSize = room.PositionBotRight - room.PositionTopLeft;
                    GameEventManager.LevelBossGenerated?.Invoke(Mathf.Min(roomSize.X, roomSize.Y) / 2, boss);
                    
                    LevelExit exit = _levelExitScene.Instantiate<LevelExit>();
                    _entitiesRoot.AddChild(exit);
                    exit.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;
                    break;
                case Room.RoomType.Treasure:
                    LootCollectable coin = _coinScene.Instantiate<LootCollectable>();
                    _entitiesRoot.AddChild(coin);
                    coin.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;
                    
                    GenerateTreasureRoomEnemies(room, CalculateEnemyAmount());
                    
                    break;
                default:
                    GD.PrintErr("Room type not implemented");
                    break;
            }
        }
    }

    private void GenerateTreasureRoomEnemies(Room room, int amount)
    {
        Vector2I roomSize = new(
            room.PositionBotRight.X - room.PositionTopLeft.X, 
            room.PositionBotRight.Y - room.PositionTopLeft.Y);
        
        for (int i = 0; i < amount && amount <= _maxEnemiesPerRoom; i++)
        {
            Vector2I startPos;
            Vector2I endPos;
            switch (i)
            {
                case 0: // patrol top
                    startPos = (room.PositionTopLeft + room.PositionBotRight) / 2 - roomSize / 4;
                    endPos = new(startPos.X + roomSize.X / 2, startPos.Y);
                    break;
                case 1: // patrol bottom
                    startPos = (room.PositionTopLeft + room.PositionBotRight) / 2 - roomSize / 4;
                    startPos += new Vector2I(0, roomSize.Y / 2);
                    endPos = new(startPos.X + roomSize.X / 2, startPos.Y);
                    break;
                case 2: // patrol left
                    startPos = (room.PositionTopLeft + room.PositionBotRight) / 2 - roomSize / 3;
                    endPos = new(startPos.X, startPos.Y + roomSize.Y / 3 * 2);
                    break;
                default: // patrol right
                    startPos = (room.PositionTopLeft + room.PositionBotRight) / 2 - roomSize / 3;
                    Vector2I relStartPos = startPos - room.PositionTopLeft;
                    startPos += new Vector2I((int)((roomSize.X - relStartPos.X) - (0.17f * roomSize.X)), 0);
                    endPos = new(startPos.X, startPos.Y + roomSize.Y / 3 * 2);
                    break;
            }

            startPos *= LevelTileMap.TileSize;
            endPos *= LevelTileMap.TileSize;

            Sprite2D debugTarget1 = _debugPatrolTarget.Instantiate<Sprite2D>();
            _entitiesRoot.AddChild(debugTarget1);
            debugTarget1.Position = startPos;
            debugTarget1.AddToGroup("debug");
            debugTarget1.Hide();
            Sprite2D debugTarget2 = _debugPatrolTarget.Instantiate<Sprite2D>();
            _entitiesRoot.AddChild(debugTarget2);
            debugTarget2.Position = endPos;
            debugTarget2.AddToGroup("debug");
            debugTarget2.Hide();
            
            
            CharacterBody2D knight = _knightEnemyScene.Instantiate<CharacterBody2D>();
            _entitiesRoot.AddChild(knight);
            knight.Position = startPos;
            EnemyPatrol patrolComponent = (EnemyPatrol)knight.FindChild("EnemyPatrol");
            patrolComponent.AddPatrolTarget(startPos);
            patrolComponent.AddPatrolTarget(endPos);

        }
    }

    private int CalculateEnemyAmount()
    {
        int diff = Mathf.Min((int)DifficultyManager.DifficultyScale, 4); // difficulty scale, max 4
        
        int randDiff = GD.RandRange(diff - 1, diff); // random from diff -1 to diff

        return Mathf.Max(randDiff, 1); // diff, min 1
    }
    
}
