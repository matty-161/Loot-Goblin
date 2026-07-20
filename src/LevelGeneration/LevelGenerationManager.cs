using Godot;
using System;

namespace LootGoblin.LevelGeneration;

public partial class LevelGenerationManager : Node
{
    [Export] private LevelTileMap _levelTileMap;
    [Export] private PartitionTreeGenerator _partitionTreeGenerator;
    [Export] private RoomGenerator _roomGenerator;
    [Export] private CorridorGenerator _corridorGenerator;
    [Export] private InteriorGenerator _interiorGenerator;
    
    private Button _generateButton;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _generateButton =  GetNode<Button>("%GenerateButton");
        _generateButton.Pressed += GenerateLevel;
    }

    private void GenerateLevel()
    {
        _partitionTreeGenerator.GeneratePartitionTree();
        _roomGenerator.GenerateRooms();
        _corridorGenerator.GenerateCorridors();
        _interiorGenerator.GenerateInteriors();
    }
}
