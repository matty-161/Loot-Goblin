using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

namespace LootGoblin.LevelGeneration;

public partial class LevelTileMap : TileMapLayer
{
	public const int TileSize = 16;

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
		SetCellsTerrainConnect(cells, 0, 0, true);
		NotifyRuntimeTileDataUpdate();
	}
}
