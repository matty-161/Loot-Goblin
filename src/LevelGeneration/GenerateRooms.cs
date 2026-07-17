using Godot;
using System;
using System.Collections.Generic;

namespace LootGoblin.LevelGeneration;

public struct Room(Vector2I positionTopLeft, Vector2I positionBotRight)
{
    public Vector2I PositionTopLeft = positionTopLeft;
    public Vector2I PositionBotRight = positionBotRight;
}

public partial class GenerateRooms : Node2D
{
    [Export] private GeneratePartitionTree _generatePartitionTree;
    [Export] private LevelTileMap _levelTileMap;

    private readonly List<Room> _rooms = [];

    public override void _Ready()
    {
        _generatePartitionTree.PartitionsGenerated += Generate;
    }

    private void Generate()
    {
        Clear();
        List<TreeNode> leafNodes = _generatePartitionTree.GetLeafNodes();
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
