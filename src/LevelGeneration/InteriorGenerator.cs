using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using LootGoblin.Enemies;
using LootGoblin.LevelGeneration.Scenes.Treasure;

namespace LootGoblin.LevelGeneration;

public partial class InteriorGenerator : Node
{
    
    private RoomGenerator _roomGenerator;
    private CorridorGenerator _corridorGenerator;

    private PackedScene _playerScene;
    private PackedScene _bossScene;
    private PackedScene _coinScene;

    public void GenerateInteriors(
        Node2D entitiesRoot,
        RoomGenerator rg,
        CorridorGenerator cg,
        PackedScene playerScene,
        PackedScene bossScene,
        PackedScene coinScene)
    {
        _roomGenerator = rg;
        _corridorGenerator = cg;
        _playerScene = playerScene;
        _bossScene = bossScene;
        _coinScene = coinScene;
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
            switch (room.Type)
            {
                case Room.RoomType.Spawn:
                    CharacterBody2D player = _playerScene.Instantiate<CharacterBody2D>();
                    entitiesRoot.AddChild(player);
                    player.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;
                    break;
                case Room.RoomType.Boss:
                    BossEnemy boss = _bossScene.Instantiate<BossEnemy>();
                    entitiesRoot.AddChild(boss);
                    boss.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;
                    break;
                case Room.RoomType.Treasure:
                    LootCollectable coin = _coinScene.Instantiate<LootCollectable>();
                    entitiesRoot.AddChild(coin);
                    coin.Position = (room.PositionTopLeft + room.PositionBotRight) / 2 * LevelTileMap.TileSize;
                    break;
                default:
                    GD.PrintErr("Room type not implemented");
                    break;
            }
        }
    }
    
}
