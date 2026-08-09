using Godot;
using System;
using Godot.Collections;
using Range = System.Range;

namespace LootGoblin;

public partial class CorridorGenerator : Node
{
	
	private PartitionTreeGenerator _partitionTreeGenerator;
	private RoomGenerator _roomGenerator;
	
	private LevelTileMap _levelTileMap;

	private TreeNode _root;

	public void GenerateCorridors(LevelTileMap ltm, PartitionTreeGenerator ptg, RoomGenerator rg)
	{
		_levelTileMap = ltm;
		_partitionTreeGenerator = ptg;
		_roomGenerator = rg;
		_root = _partitionTreeGenerator.GetRoot();
		TraverseTree(_root);
	}

	private void TraverseTree(TreeNode node)
	{
		// base case is leaf node
		if (node.Left == null) return;
		
		ConnectChildren(node);
		
		TraverseTree(node.Left);
		TraverseTree(node.Right);
		
	}

	private void ConnectChildren(TreeNode node)
	{
		Array<Vector2I> cells = [];
		
		// if children are leaf node i.e. they contain rooms, connect the rooms
		if (node.Left.Room != null && node.Right.Room != null)
		{
			Vector2I leftRoomDimensions = node.Left.Room.PositionBotRight - node.Left.Room.PositionTopLeft;
			Vector2I leftRoomCentre = node.Left.Room.PositionTopLeft + leftRoomDimensions / 2;

			Vector2I rightRoomDimensions = node.Right.Room.PositionBotRight - node.Right.Room.PositionTopLeft;
			Vector2I rightRoomCentre = node.Right.Room.PositionTopLeft + rightRoomDimensions / 2;

			if (node.IsSplitVertical) // if the partitions are split vertically, make horizontal corridors
			{
				// first go along X to right room
				for (int x = leftRoomCentre.X; x < rightRoomCentre.X + 2; x++)
				{
					cells.Add(new(x, leftRoomCentre.Y));
					cells.Add(new(x, leftRoomCentre.Y + 1));
					cells.Add(new(x, leftRoomCentre.Y - 1));
				}
				// then turn a corner and go up or down
				if (leftRoomCentre.Y < rightRoomCentre.Y) // if left room is above right room go down
				{
					for (int y = leftRoomCentre.Y; y < rightRoomCentre.Y; y++)
					{
						cells.Add(new(rightRoomCentre.X, y));
						cells.Add(new(rightRoomCentre.X + 1, y));
						cells.Add(new(rightRoomCentre.X - 1, y));
					}
				}
				else // else go up
				{
					for (int y = leftRoomCentre.Y; y > rightRoomCentre.Y; y--)
					{
						cells.Add(new(rightRoomCentre.X, y));
						cells.Add(new(rightRoomCentre.X + 1, y));
						cells.Add(new(rightRoomCentre.X - 1, y));
					}
				}
			}
			else // else the partitions are split horizontally, make vertical corridors 
			{
				// first go along Y to right (lower) room
				for (int y = leftRoomCentre.Y; y < rightRoomCentre.Y + 2; y++)
				{
					cells.Add(new(leftRoomCentre.X, y));
					cells.Add(new(leftRoomCentre.X + 1, y));
					cells.Add(new(leftRoomCentre.X - 1, y));
				}
				// then turn a corner and go left or right
				if (leftRoomCentre.X < rightRoomCentre.X) // if left (upper) room is to the left of right (lower) room go right 
				{
					for (int x = leftRoomCentre.X; x < rightRoomCentre.X; x++)
					{
						cells.Add(new(x, rightRoomCentre.Y));
						cells.Add(new(x, rightRoomCentre.Y + 1));
						cells.Add(new(x, rightRoomCentre.Y - 1));
					}
				}
				else // else go left
				{
					for (int x = leftRoomCentre.X; x > rightRoomCentre.X; x--)
					{
						cells.Add(new(x, rightRoomCentre.Y));
						cells.Add(new(x, rightRoomCentre.Y + 1));
						cells.Add(new(x, rightRoomCentre.Y - 1));
					}
				}
			}
		}
		else
		{
			Vector2I leftChildCentre = node.Left.Position + node.Left.Dimensions / 2;
			Vector2I rightChildCentre = node.Right.Position + node.Left.Dimensions / 2;
		
			if (node.IsSplitVertical) // if the partitions are split vertically, make horizontal corridors
			{
				for (int x = leftChildCentre.X; x < rightChildCentre.X; x++)
				{
					cells.Add(new(x, leftChildCentre.Y));
					cells.Add(new(x, leftChildCentre.Y + 1));
					cells.Add(new(x, leftChildCentre.Y - 1));
				}
			}
			else // else the partitions are split horizontally, make vertical corridors 
			{
				for (int y = leftChildCentre.Y; y < rightChildCentre.Y; y++)
				{
					cells.Add(new(rightChildCentre.X, y));
					cells.Add(new(rightChildCentre.X + 1, y));
					cells.Add(new(rightChildCentre.X - 1, y));
				}
			}
		}

		_levelTileMap.PlaceCorridor(cells);
	}
}
