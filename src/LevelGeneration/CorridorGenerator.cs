using Godot;
using System;
using Godot.Collections;
using Range = System.Range;

namespace LootGoblin.LevelGeneration;

public partial class CorridorGenerator : Node2D
{
	[Export] private PartitionTreeGenerator _partitionTreeGenerator;
	[Export] private RoomGenerator _roomGenerator;
	[Export] private LevelTileMap _levelTileMap;

	private TreeNode _root;

	public override void _Ready()
	{
		_roomGenerator.RoomsGenerated += GenerateCorridors;
	}

	private void GenerateCorridors()
	{

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
			}
		}
		else // else the partitions are split horizontally, make vertical corridors 
		{
			for (int y = leftChildCentre.Y; y < rightChildCentre.Y; y++)
			{
				cells.Add(new Vector2I(rightChildCentre.X, y));
				cells.Add(new Vector2I(rightChildCentre.X - 1, y));
			}
		}

		_levelTileMap.PlaceCorridor(cells);

	}
}
