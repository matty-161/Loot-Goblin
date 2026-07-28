using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using LootGoblin.Autoloads;
using LootGoblin.Systems;

namespace LootGoblin.LevelGeneration;

public partial class LevelTileMap : TileMapLayer
{
	[Export] private LevelProgressionManager _levelProgressionManager;
	
	public const int TileSize = 16;
	
	private int _terrainIndex = 0;

	public override void _Ready()
	{
		GameplaySignalBus.Instance.LevelTransitionEvent += ChangeTerrain;
	}
	
	public void PlaceRoom(Room room)
	{
		Vector2I roomDimensions = new(
			room.PositionBotRight.X - room.PositionTopLeft.X,
			room.PositionBotRight.Y - room.PositionTopLeft.Y
		);

		Array<Vector2I> cells = [];
		for (int x = 0; x < roomDimensions.X; x++)
		{
			for (int y = 0; y < roomDimensions.Y; y++)
			{
				cells.Add(new Vector2I(room.PositionTopLeft.X + x, room.PositionTopLeft.Y + y));
			}
		}
		SetCellsTerrainConnect(cells, 0, _terrainIndex);
		NotifyRuntimeTileDataUpdate();
	}

	public void PlaceCorridor(Array<Vector2I> cells)
	{
		SetCellsTerrainConnect(cells, 0, _terrainIndex);
		NotifyRuntimeTileDataUpdate();
	}

	private void ChangeTerrain()
	{
		// subtract 1 from CurrentLevel as CurrentLevel starts at 1, but the terrain index starts at 0
		_terrainIndex = _levelProgressionManager.CurrentLevel - 1; 
	}
}
