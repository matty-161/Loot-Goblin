using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using LootGoblin.Characters.Enemies.States;
using LootGoblin.LevelGeneration.LevelContents;
using LootGoblin.LevelGeneration.LevelContents.Treasure;

namespace LootGoblin.LevelGeneration;

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
        SelectRooms();
        PlaceRoomStuff();
    }

    private void SelectRooms()
    {
        List<Room> rooms = _roomGenerator.GetRooms();
        List<Room> roomsCopy = new List<Room>(rooms);
        
        // randomly choose room to be spawn room, then remove from list so it can't be overridden
        int spawnIndex = (int)(GD.Randi() % roomsCopy.Count);
        roomsCopy[spawnIndex].Type = Room.RoomType.Spawn;
        roomsCopy.RemoveAt(spawnIndex);
        
        // randomly choose room to be boss room, then remove from list so it can't be overridden
        int bossIndex = (int)(GD.Randi() % roomsCopy.Count);
        roomsCopy[bossIndex].Type = Room.RoomType.Boss;
        roomsCopy.RemoveAt(bossIndex);
        
        // make the rest of the rooms treasure rooms
        foreach (Room room in roomsCopy)
        {
            room.Type = Room.RoomType.Treasure;
        }
    }

    private void PlaceRoomStuff()
    {
        List<Room> rooms = _roomGenerator.GetRooms();
        
        foreach (Room room in rooms)
        {
            
            switch (room.Type)
            {
                
                case Room.RoomType.Spawn:
                    CharacterBody2D player = _playerScene.Instantiate<CharacterBody2D>();
                    _entitiesRoot.AddChild(player);
                    player.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;
                    break;
                case Room.RoomType.Boss:
                    CharacterBody2D boss = _bossScene.Instantiate<CharacterBody2D>();
                    _entitiesRoot.AddChild(boss);
                    boss.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;
                    
                    LevelExit exit = _levelExitScene.Instantiate<LevelExit>();
                    _entitiesRoot.AddChild(exit);
                    exit.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;
                    break;
                case Room.RoomType.Treasure:
                    LootCollectable coin = _coinScene.Instantiate<LootCollectable>();
                    _entitiesRoot.AddChild(coin);
                    coin.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;
                    
                    GenerateTreasureRoomEnemies(room, 1);
                    
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
                    startPos += new Vector2I(startPos.X, startPos.Y + roomSize.Y / 2);
                    endPos = new(startPos.X + roomSize.X / 2, startPos.Y);
                    break;
                case 2: // patrol left
                    startPos = (room.PositionTopLeft + room.PositionBotRight) / 2 - roomSize / 3;
                    endPos = new(startPos.X, startPos.Y + roomSize.Y / 3 * 2);
                    break;
                default: // patrol right
                    startPos = (room.PositionTopLeft + room.PositionBotRight) / 2 - roomSize / 3;
                    startPos += new Vector2I(startPos.X + roomSize.X / 3, roomSize.Y);
                    endPos = new(startPos.X, startPos.Y + roomSize.Y / 3 * 2);
                    break;
            }

            startPos *= LevelTileMap.TileSize;
            endPos *= LevelTileMap.TileSize;

            Sprite2D debugTarget1 = _debugPatrolTarget.Instantiate<Sprite2D>();
            _entitiesRoot.AddChild(debugTarget1);
            debugTarget1.Position = startPos;
            Sprite2D debugTarget2 = _debugPatrolTarget.Instantiate<Sprite2D>();
            _entitiesRoot.AddChild(debugTarget2);
            debugTarget2.Position = endPos;
            
            
            CharacterBody2D knight = _knightEnemyScene.Instantiate<CharacterBody2D>();
            _entitiesRoot.AddChild(knight);
            knight.Position = startPos;
            EnemyPatrol patrolComponent = (EnemyPatrol)knight.FindChild("EnemyPatrol");
            patrolComponent.AddPatrolTarget(startPos);
            patrolComponent.AddPatrolTarget(endPos);

        }
    }
    
}
