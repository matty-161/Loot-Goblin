using Godot;
using System;

namespace LootGoblin.LevelGeneration;

public partial class LevelGenerationManager : Node
{
    [ExportCategory("Generation Settings")]
    [Export] private Vector2I _levelSize = new(50, 50);
    [Export] private int _minPartitionSize = 10;
    [Export] private int _maxPartitionDivision = 4; // this is the most a partition can be divided by
    
    [ExportCategory("Spawnable Scenes")]
    [Export] private PackedScene _playerScene;
    [Export] private PackedScene _bossScene;
    [Export] private PackedScene _coinScene;
    
    [ExportCategory("Node References")]
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
        _partitionTreeGenerator.GeneratePartitionTree(_levelSize, _minPartitionSize, _maxPartitionDivision);
        _roomGenerator.GenerateRooms(_levelTileMap, _partitionTreeGenerator);
        _corridorGenerator.GenerateCorridors(_levelTileMap, _partitionTreeGenerator, _roomGenerator);
        _interiorGenerator.GenerateInteriors(
            _entitiesRoot, _roomGenerator, _corridorGenerator,
            _playerScene, _bossScene, _coinScene);
    }
}
