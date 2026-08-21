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
		
		// connect the tree from the bottom up
		for (int d = _partitionTreeGenerator.GetMaxDepth(); d >= 0; d--)
		{
			TraverseTree(_root, d);
		}
	}

	private void TraverseTree(TreeNode node, int depth)
	{
		// base case is leaf node
		if (node.Left == null) return;
		
		// incorrect depth, continue traversal without connecting children
		if (node.Depth != depth)
		{
			TraverseTree(node.Left, depth);
			TraverseTree(node.Right, depth);
			return;
		}
		
		ConnectChildren(node);
		TraverseTree(node.Left, depth);
		TraverseTree(node.Right, depth);
	}

	private void ConnectChildren(TreeNode node)
	{
		Array<Vector2I> cells = [];
		
		// if children are leaf node i.e. they contain rooms, connect the rooms
		if (node.Left.Room != null && node.Right.Room != null)
		{
			ConnectLeafNodes(node, cells);
		}
		// if both children are internal nodes, connect the corridors
		else if (node.Left.Corridor != null && node.Right.Corridor != null) 
		{
			ConnectInternalNodes(node, cells);
		}
		// if left node is a leaf node and the right node is an internal node, connect the left room to right corridor
		else if (node.Left.Room != null && node.Right.Corridor != null)
		{
			ConnectLeftRoomRightCorridor(node, cells);
		}
		// if left node is an internal node and the right node is a leaf node, connect the left corridor to right room
		else if (node.Left.Corridor != null && node.Right.Room != null)
		{
			ConnectLeftCorridorRightRoom(node, cells);
		}
		else
		{
			GD.PushError("Could not connect children. No rooms or corridors found to connect.");
		}

		_levelTileMap.PlaceCorridor(cells);
	}

	private void ConnectLeafNodes(TreeNode node, Array<Vector2I> cells)
	{
		Vector2I leftRoomDimensions = node.Left.Room.PositionBotRight - node.Left.Room.PositionTopLeft;
		Vector2I leftRoomCentre = node.Left.Room.PositionTopLeft + leftRoomDimensions / 2;

		Vector2I rightRoomDimensions = node.Right.Room.PositionBotRight - node.Right.Room.PositionTopLeft;
		Vector2I rightRoomCentre = node.Right.Room.PositionTopLeft + rightRoomDimensions / 2;

		if (node.IsSplitVertical) // if the partitions are split vertically, make horizontal corridors
		{
			node.Corridor = new(leftRoomCentre, new(rightRoomCentre.X, leftRoomCentre.Y));
			
			// first go along X to right room
			for (int x = leftRoomCentre.X - 1; x < rightRoomCentre.X + 2; x++)
			{
				cells.Add(new(x, leftRoomCentre.Y));
				cells.Add(new(x, leftRoomCentre.Y + 1));
				cells.Add(new(x, leftRoomCentre.Y - 1));
			}
			// then turn a corner and go up or down
			if (leftRoomCentre.Y < rightRoomCentre.Y) // if left room is above right room go down
			{
				for (int y = leftRoomCentre.Y - 1; y < rightRoomCentre.Y + 2; y++)
				{
					cells.Add(new(rightRoomCentre.X, y));
					cells.Add(new(rightRoomCentre.X + 1, y));
					cells.Add(new(rightRoomCentre.X - 1, y));
				}
			}
			else // else go up
			{
				for (int y = leftRoomCentre.Y + 1; y >= rightRoomCentre.Y - 2; y--)
				{
					cells.Add(new(rightRoomCentre.X, y));
					cells.Add(new(rightRoomCentre.X + 1, y));
					cells.Add(new(rightRoomCentre.X - 1, y));
				}
			}
		}
		else // else the partitions are split horizontally, make vertical corridors 
		{
			node.Corridor = new(leftRoomCentre, new(leftRoomCentre.X, rightRoomCentre.Y));
			
			// first go along Y to right (lower) room
			for (int y = leftRoomCentre.Y - 1; y < rightRoomCentre.Y + 2; y++)
			{
				cells.Add(new(leftRoomCentre.X, y));
				cells.Add(new(leftRoomCentre.X + 1, y));
				cells.Add(new(leftRoomCentre.X - 1, y));
			}
			// then turn a corner and go left or right
			if (leftRoomCentre.X < rightRoomCentre.X) // if left (upper) room is to the left of right (lower) room go right 
			{
				for (int x = leftRoomCentre.X - 1; x < rightRoomCentre.X + 2; x++)
				{
					cells.Add(new(x, rightRoomCentre.Y));
					cells.Add(new(x, rightRoomCentre.Y + 1));
					cells.Add(new(x, rightRoomCentre.Y - 1));
				}
			}
			else // else go left
			{
				for (int x = leftRoomCentre.X + 1; x >= rightRoomCentre.X - 2; x--)
				{
					cells.Add(new(x, rightRoomCentre.Y));
					cells.Add(new(x, rightRoomCentre.Y + 1));
					cells.Add(new(x, rightRoomCentre.Y - 1));
				}
			}
		}
	}

	private void ConnectInternalNodes(TreeNode node, Array<Vector2I> cells)
	{
		Vector2I leftCorridorDimensions = node.Left.Corridor.PositionBotRight - node.Left.Corridor.PositionTopLeft;
		Vector2I leftCorridorCentre = node.Left.Corridor.PositionTopLeft + leftCorridorDimensions / 2;
		
		Vector2I rightCorridorDimensions = node.Right.Corridor.PositionBotRight - node.Right.Corridor.PositionTopLeft;
		Vector2I rightCorridorCentre = node.Right.Corridor.PositionTopLeft + rightCorridorDimensions / 2;
		
		
		if (node.IsSplitVertical) // if the partitions are split vertically, make horizontal corridors
		{
			node.Corridor = new(leftCorridorCentre, new(leftCorridorCentre.X, rightCorridorCentre.Y));
			
			// first go along x from left to right
			for (int x = leftCorridorCentre.X - 1; x < rightCorridorCentre.X + 2; x++)
			{
				cells.Add(new(x, leftCorridorCentre.Y));
				cells.Add(new(x, leftCorridorCentre.Y + 1));
				cells.Add(new(x, leftCorridorCentre.Y - 1));
			}
			// then turn a corner and go up or down
			if (leftCorridorCentre.Y < rightCorridorCentre.Y) // if left corridor is above right corridor go down
			{
				for (int y = leftCorridorCentre.Y - 1; y < rightCorridorCentre.Y + 2; y++)
				{
					cells.Add(new(leftCorridorCentre.X, y));
					cells.Add(new(leftCorridorCentre.X + 1, y));
					cells.Add(new(leftCorridorCentre.X - 1, y));
				}
			}
			else // else go up
			{
				for (int y = leftCorridorCentre.Y + 1; y >= rightCorridorCentre.Y - 2; y--)
				{
					cells.Add(new(leftCorridorCentre.X, y));
					cells.Add(new(leftCorridorCentre.X + 1, y));
					cells.Add(new(leftCorridorCentre.X - 1, y));
				}
			}
		}
		else // else the partitions are split horizontally, make vertical corridors 
		{
			node.Corridor = new(leftCorridorCentre, new(leftCorridorCentre.X, rightCorridorCentre.Y));
			
			for (int y = leftCorridorCentre.Y - 1; y < rightCorridorCentre.Y + 2; y++)
			{
				cells.Add(new(leftCorridorCentre.X, y));
				cells.Add(new(leftCorridorCentre.X + 1, y));
				cells.Add(new(leftCorridorCentre.X - 1, y));
			}

			if (leftCorridorCentre.X < rightCorridorCentre.X) // if left (upper) room is to the left of right (lower) room go right 
			{
				for (int x = leftCorridorCentre.X - 1; x < rightCorridorCentre.X + 2; x++)
				{
					cells.Add(new(x, leftCorridorCentre.Y));
					cells.Add(new(x, leftCorridorCentre.Y + 1));
					cells.Add(new(x, leftCorridorCentre.Y - 1));
				}
			}
			else // else go left
			{
				for (int x = leftCorridorCentre.X + 1; x >= rightCorridorCentre.X - 2; x--)
				{
					cells.Add(new(x, leftCorridorCentre.Y));
					cells.Add(new(x, leftCorridorCentre.Y + 1));
					cells.Add(new(x, leftCorridorCentre.Y - 1));
				}
			}
		}
	}

	private void ConnectLeftRoomRightCorridor(TreeNode node, Array<Vector2I> cells)
	{
		Vector2I leftRoomDimensions = node.Left.Room.PositionBotRight - node.Left.Room.PositionTopLeft;
		Vector2I leftRoomCentre = node.Left.Room.PositionTopLeft + leftRoomDimensions / 2;
		
		Vector2I rightCorridorDimensions = node.Right.Corridor.PositionBotRight - node.Right.Corridor.PositionTopLeft;
		Vector2I rightCorridorCentre = node.Right.Corridor.PositionTopLeft + rightCorridorDimensions / 2;

		if (node.IsSplitVertical) // if the partitions are split vertically, make horizontal corridors
		{
			node.Corridor = new(new(leftRoomCentre.X, rightCorridorCentre.Y), rightCorridorCentre);
			
			// first go along x from right to left
			for (int x = rightCorridorCentre.X + 1; x >= leftRoomCentre.X + 2; x--)
			{
				cells.Add(new(x, rightCorridorCentre.Y));
				cells.Add(new(x, rightCorridorCentre.Y + 1));
				cells.Add(new(x, rightCorridorCentre.Y - 1));
			}
			// then turn a corner and go up or down
			if (leftRoomCentre.Y > rightCorridorCentre.Y) // if left room is below right corridor go down
			{
				for (int y = rightCorridorCentre.Y - 1; y < leftRoomCentre.Y + 2; y++)
				{
					cells.Add(new(leftRoomCentre.X, y));
					cells.Add(new(leftRoomCentre.X + 1, y));
					cells.Add(new(leftRoomCentre.X - 1, y));
				}
			}
			else // else go up
			{
				for (int y = rightCorridorCentre.Y + 1; y >= leftRoomCentre.Y - 2; y--)
				{
					cells.Add(new(leftRoomCentre.X, y));
					cells.Add(new(leftRoomCentre.X + 1, y));
					cells.Add(new(leftRoomCentre.X - 1, y));
				}
			}
		}
		else // else the partitions are split horizontally, make vertical corridors 
		{
			node.Corridor = new(new(leftRoomCentre.X, rightCorridorCentre.Y), rightCorridorCentre);
			
			// first go along y from bottom corridor to top room
			for (int y = rightCorridorCentre.Y + 1; y >= leftRoomCentre.Y - 2; y--)
			{
				cells.Add(new(rightCorridorCentre.X, y));
				cells.Add(new(rightCorridorCentre.X + 1, y));
				cells.Add(new(rightCorridorCentre.X - 1, y));
			}
			// then turn a corner and go left or right
			if (leftRoomCentre.X > rightCorridorCentre.X) // if left (upper) room is to the right of right (lower) corridor go right 
			{
				for (int x = rightCorridorCentre.X - 1; x < leftRoomCentre.X + 2; x++)
				{
					cells.Add(new(x, leftRoomCentre.Y));
					cells.Add(new(x, leftRoomCentre.Y + 1));
					cells.Add(new(x, leftRoomCentre.Y - 1));
				}
			}
			else // else go left
			{
				for (int x = rightCorridorCentre.X + 1; x >= leftRoomCentre.X - 2; x--)
				{
					cells.Add(new(x, leftRoomCentre.Y));
					cells.Add(new(x, leftRoomCentre.Y + 1));
					cells.Add(new(x, leftRoomCentre.Y - 1));
				}
			}
		}
	}
	
	private void ConnectLeftCorridorRightRoom(TreeNode node, Array<Vector2I> cells)
	{
		Vector2I leftCorridorDimensions = node.Left.Corridor.PositionBotRight - node.Left.Corridor.PositionTopLeft;
		Vector2I leftCorridorCentre = node.Left.Corridor.PositionTopLeft + leftCorridorDimensions / 2;
		
		Vector2I rightRoomDimensions = node.Right.Room.PositionBotRight - node.Right.Room.PositionTopLeft;
		Vector2I rightRoomCentre = node.Right.Room.PositionTopLeft + rightRoomDimensions / 2;
		
		if (node.IsSplitVertical) // if the partitions are split vertically, make horizontal corridors
		{
			node.Corridor = new(new(rightRoomCentre.X, leftCorridorCentre.Y), leftCorridorCentre);
			
			// first go along x from left corridor to right room
			for (int x = leftCorridorCentre.X - 1; x < rightRoomCentre.X + 2; x++)
			{
				cells.Add(new(x, leftCorridorCentre.Y));
				cells.Add(new(x, leftCorridorCentre.Y + 1));
				cells.Add(new(x, leftCorridorCentre.Y - 1));
			}
			// then turn a corner and go up or down
			if (rightRoomCentre.Y > leftCorridorCentre.Y)
			{
				for (int y = leftCorridorCentre.Y - 1; y < rightRoomCentre.Y + 2; y++) // go down from left corridor to right room
				{
					cells.Add(new(rightRoomCentre.X, y));
					cells.Add(new(rightRoomCentre.X + 1, y));
					cells.Add(new(rightRoomCentre.X - 1, y));
				}
			}
			else
			{
				for (int y = leftCorridorCentre.Y + 1; y >= rightRoomCentre.Y - 2; y--)
				{
					cells.Add(new(rightRoomCentre.X, y));
					cells.Add(new(rightRoomCentre.X + 1, y));
					cells.Add(new(rightRoomCentre.X - 1, y));
				}
			}
		}
		else // else the partitions are split horizontally, make vertical corridors 
		{
			node.Corridor = new(new(rightRoomCentre.X, leftCorridorCentre.Y), leftCorridorCentre);
			
			// first go along y from left corridor to right room
			for (int y = leftCorridorCentre.Y + 1; y >= rightRoomCentre.Y - 2; y--)
			{
				cells.Add(new(leftCorridorCentre.X, y));
				cells.Add(new(leftCorridorCentre.X + 1, y));
				cells.Add(new(leftCorridorCentre.X - 1, y));
			}
			
			// then turn a corner and go left or right
			if (rightRoomCentre.X > leftCorridorCentre.X)
			{
				for (int x = leftCorridorCentre.X - 1; x < rightRoomCentre.X + 2; x++)
				{
					cells.Add(new(x, rightRoomCentre.Y));
					cells.Add(new(x, rightRoomCentre.Y + 1));
					cells.Add(new(x, rightRoomCentre.Y - 1));
				}
			}
			else
			{
				for (int x = leftCorridorCentre.X + 1; x >= rightRoomCentre.X - 2; x--)
				{
					cells.Add(new(x, rightRoomCentre.Y));
					cells.Add(new(x, rightRoomCentre.Y + 1));
					cells.Add(new(x, rightRoomCentre.Y - 1));
				}
			}
		}
	}
}

public class Corridor(Vector2I positionTopLeft, Vector2I positionBotRight)
{
	public Vector2I PositionTopLeft = positionTopLeft;
	public Vector2I PositionBotRight = positionBotRight;
}
