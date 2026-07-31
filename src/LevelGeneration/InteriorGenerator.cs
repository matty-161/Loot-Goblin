using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using LootGoblin.LevelGeneration.LevelContents;
using LootGoblin.LevelGeneration.LevelContents.Treasure;

namespace LootGoblin.LevelGeneration;

public partial class InteriorGenerator : Node
{
    
    private RoomGenerator _roomGenerator;
    private CorridorGenerator _corridorGenerator;

    [Export] private PackedScene _playerScene;
    [Export] private PackedScene _bossScene;
    [Export] private PackedScene _levelExitScene;
    [Export] private PackedScene _coinScene;
    [Export] private PackedScene _knightEnemyScene;

    public void GenerateInteriors(
        Node2D entitiesRoot,
        RoomGenerator rg,
        CorridorGenerator cg)
    {
        _roomGenerator = rg;
        _corridorGenerator = cg;
        SelectRooms();
        PlaceRoomStuff(entitiesRoot);
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

    private void PlaceRoomStuff(Node2D entitiesRoot)
    {
        List<Room> rooms = _roomGenerator.GetRooms();
        
        foreach (Room room in rooms)
        {
            Vector2I roomSize = new(room.PositionBotRight.X - room.PositionTopLeft.X, room.PositionBotRight.Y - room.PositionTopLeft.Y);
            switch (room.Type)
            {
                
                case Room.RoomType.Spawn:
                    CharacterBody2D player = _playerScene.Instantiate<CharacterBody2D>();
                    entitiesRoot.AddChild(player);
                    player.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;
                    break;
                case Room.RoomType.Boss:
                    CharacterBody2D boss = _bossScene.Instantiate<CharacterBody2D>();
                    entitiesRoot.AddChild(boss);
                    boss.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;
                    
                    LevelExit exit = _levelExitScene.Instantiate<LevelExit>();
                    entitiesRoot.AddChild(exit);
                    exit.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;
                    break;
                case Room.RoomType.Treasure:
                    LootCollectable coin = _coinScene.Instantiate<LootCollectable>();
                    entitiesRoot.AddChild(coin);
                    coin.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;

                    CharacterBody2D knight = _knightEnemyScene.Instantiate<CharacterBody2D>();
                    entitiesRoot.AddChild(knight);
                    knight.Position = ((room.PositionTopLeft + room.PositionBotRight) / 2 - roomSize / 4) * LevelTileMap.TileSize;
                    break;
                default:
                    GD.PrintErr("Room type not implemented");
                    break;
            }
        }
    }
    
}
