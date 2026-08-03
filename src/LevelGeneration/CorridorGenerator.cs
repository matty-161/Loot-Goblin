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
		Vector2I leftChildCentre = new(
			node.Left.Position.X + node.Left.Dimensions.X / 2,
			node.Left.Position.Y + node.Left.Dimensions.Y / 2
		);
		
		Vector2I rightChildCentre = new(
			node.Right.Position.X + node.Right.Dimensions.X / 2,
			node.Right.Position.Y + node.Right.Dimensions.Y / 2
		);
		
		
		Array<Vector2I> cells = [];
		if (node.IsSplitVertical) // if the partitions are split vertically, make horizontal corridors
		{
			for (int x = leftChildCentre.X; x < rightChildCentre.X; x++)
			{
				cells.Add(new Vector2I(x, leftChildCentre.Y));
				cells.Add(new Vector2I(x, leftChildCentre.Y + 1));
				cells.Add(new Vector2I(x, leftChildCentre.Y - 1));
			}
		}
		else // else the partitions are split horizontally, make vertical corridors 
		{
			for (int y = leftChildCentre.Y; y < rightChildCentre.Y; y++)
			{
				cells.Add(new Vector2I(rightChildCentre.X, y));
				cells.Add(new Vector2I(rightChildCentre.X + 1, y));
				cells.Add(new Vector2I(rightChildCentre.X - 1, y));
			}
		}

		_levelTileMap.PlaceCorridor(cells);

	}
}
