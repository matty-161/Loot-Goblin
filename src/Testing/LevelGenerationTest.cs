using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Godot;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace LootGoblin;

public partial class LevelGenerationTest : Node
{
    [Export] private Button _generateOnceButton;
    [Export] private Button _testButton;
    [Export] private DebugLineEdit _iterationsEdit;

    [Export] private LevelGenerationManager _generationManager;

    private Timer _timer;
    private int _iterationsCount;

    public static RoomNavigabilityEvaluator NavigabilityEvaluator { get; set; }
    public static List<Room> Rooms { get; set; }

    public override void _Ready()
    {
        _generateOnceButton.Pressed += GenerateOnce;
        _testButton.Pressed += RunTests;
        _iterationsEdit.DebugTextChanged += OnDebugTextChanged;
    }

    public override void _ExitTree()
    {
        _generateOnceButton.Pressed -= GenerateOnce;
        _testButton.Pressed -= RunTests;
        _iterationsEdit.DebugTextChanged -= OnDebugTextChanged;
    }

    private void GenerateOnce()
    {
        // generate the level
        _generationManager.GenerateLevel(true);
    }

    private async void RunTests()
    {
        // setup csv stuff
        string directory = "Testing/Tests/";
        string testFileName = "generation_test_v3.csv";
        using StreamWriter writer = new(directory + testFileName);
        
        using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);
        csv.WriteHeader<Test>();
        
        // do tests
        for (int i = 0; i < _iterationsCount; i++)
        {
            // generate the level
            _generationManager.GenerateLevel(true);
            
            // wait for nav server to wake up
            await ToSignal(GetTree().CreateTimer(.25), Timer.SignalName.Timeout);

            // run the test
            Test test = RunTestInstance(i);
            
            // write the test to the csv file
            csv.NextRecord();
            csv.WriteRecord(test);
        }
        
        
    }

    private Test RunTestInstance(int testId)
    {
        
        int roomsAmount = Rooms.Count;
        int reachableRooms = 0;
        int unreachableRooms = 0;
        
        foreach (Room room in Rooms)
        {
            NavigabilityEvaluator.Target = 
                (room.PositionTopLeft + room.PositionBotRight)
                / 2 
                * LevelTileMap.TileSize;
        
            if (NavigabilityEvaluator.CanReachTarget())
            {
                reachableRooms += 1;
            }
            else
            {
                unreachableRooms += 1;
            }
        }
        
        Test test = new()
        {
            TestId = testId, 
            RoomsAmount = roomsAmount,
            ReachableRooms = reachableRooms, 
            UnreachableRooms = unreachableRooms
        };

        return test;
    }
    private void OnDebugTextChanged(int num)
    {
        _iterationsCount = num;
    }
}


public class Test()
{
    [Name("TestId")]
    public int TestId { get; set; }
    
    [Name("RoomsAmount")]
    public int RoomsAmount { get; set; }
    
    [Name("ReachableRooms")]
    public int ReachableRooms { get; set; }
    
    [Name("UnreachableRooms")]
    public int UnreachableRooms { get; set; }
    
}
