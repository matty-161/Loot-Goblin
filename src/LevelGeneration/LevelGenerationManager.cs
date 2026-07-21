using Godot;
using System;

namespace LootGoblin.LevelGeneration;

public partial class LevelGenerationManager : Node
{
    [Export] private LevelTileMap _levelTileMap;
    [Export] private Node2D _entitiesRoot;
    
    [ExportCategory("Children")]
    [Export] private PartitionTreeGenerator _partitionTreeGenerator;
    [Export] private RoomGenerator _roomGenerator;
    [Export] private CorridorGenerator _corridorGenerator;
    [Export] private InteriorGenerator _interiorGenerator;
    
    private Button _generateButton;
    
    // // Called when the node enters the scene tree for the first time.
    // public override void _Ready()
    // {
    //     _generateButton =  GetNode<Button>("%GenerateButton");
    //     _generateButton.Pressed += GenerateLevel;
    // }

    public void GenerateLevel()
    {
        _partitionTreeGenerator.GeneratePartitionTree();
        _roomGenerator.GenerateRooms(_levelTileMap);
        _corridorGenerator.GenerateCorridors(_levelTileMap);
        _interiorGenerator.GenerateInteriors(_entitiesRoot);
    }
}
