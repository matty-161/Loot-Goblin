using Godot;
using System;
using System.Collections.Generic;

namespace LootGoblin.LevelGeneration;

public class Room(Vector2I positionTopLeft, Vector2I positionBotRight)
{
    public enum RoomType
    {
        Spawn,
        Boss,
        Treasure
    }

    public RoomType Type;
    public Vector2I PositionTopLeft = positionTopLeft;
    public Vector2I PositionBotRight = positionBotRight;
}

public partial class RoomGenerator : Node2D
{
    
    [Export] private PartitionTreeGenerator _partitionTreeGenerator;
    [Export] private LevelTileMap _levelTileMap;

    private readonly List<Room> _rooms = [];

    public void GenerateRooms()
    {
        Clear();
        List<TreeNode> leafNodes = _partitionTreeGenerator.GetLeafNodes();
        for (int i = 0; i < leafNodes.Count; i++)
        {
            Vector2I roomPositionTopLeft = new(
                (int)(GD.Randi() % (leafNodes[i].Dimensions.X / 2) + leafNodes[i].Position.X),
                (int)(GD.Randi() % (leafNodes[i].Dimensions.Y / 2) + leafNodes[i].Position.Y)
            );

            Vector2I partitionPosBotRight = leafNodes[i].Position + leafNodes[i].Dimensions;
            Vector2I partitionRelPosBottomRight = partitionPosBotRight - leafNodes[i].Position;

            Vector2I roomPositionBotRight = new(
                (partitionRelPosBottomRight.X / 2) + roomPositionTopLeft.X,
                (partitionRelPosBottomRight.Y / 2) + roomPositionTopLeft.Y
            );

            Room room = new(roomPositionTopLeft, roomPositionBotRight);
            _rooms.Add(room);
            _levelTileMap.PlaceRoom(room);
            leafNodes[i].Room = room;
        }
    }

    private void Clear()
    {
        _rooms.Clear();
    }

    public List<Room> GetRooms()
    {
        return _rooms;
    }

}
