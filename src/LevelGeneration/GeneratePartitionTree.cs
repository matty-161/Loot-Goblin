using System;
using System.Collections.Generic;
using Godot;
namespace LootGoblin.LevelGeneration;

public partial class GeneratePartitionTree : Node2D
{
	[Export] private Vector2I _levelSize = new(100, 100);
	[Export] private int _minPartitionSize = 10;
	[Export] private int _maxPartitionDivision = 4; // this is the most a partition can be divided by
	private int _minDivisiblePartitionSize;

	private Button _generateButton;
	
	private TreeNode _root;
	private readonly List<TreeNode> _nodes = [];
	private readonly List<TreeNode> _leafNodes = [];
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_generateButton =  GetNode<Button>("%GenerateButton");

		_generateButton.Pressed += GenerateTree;

	}

	public override void _Draw()
	{
		DrawRect(new Rect2(new Vector2(0,0), _levelSize * 16), new (0,0,0));

		for (int i = 0; i < _leafNodes.Count; i++)
		{
			DrawRect(new Rect2(_leafNodes[i].Position * 16, _leafNodes[i].Dimensions * 16), new Color(GD.Randf(), GD.Randf(), GD.Randf()));
			DrawString(ThemeDB.FallbackFont, (_leafNodes[i].Position + _leafNodes[i].Dimensions / 2) * 16, _leafNodes[i].Dimensions.ToString());
		}
	}

	private void GenerateTree()
	{
		Clear();
		_minDivisiblePartitionSize = _minPartitionSize * (_maxPartitionDivision - 1);
		_root = new(new(0, 0), _levelSize, 0);
		SplitRecursive(_root);
		FindLeafNodes(_root);
		QueueRedraw();
	}

	private void SplitRecursive(TreeNode node)
	{
		if (node.Dimensions.X < _minDivisiblePartitionSize && node.Dimensions.Y < _minDivisiblePartitionSize) return;
		
		SplitCell(node);
		SplitRecursive(node.Left);
		SplitRecursive(node.Right);
		
	}

	private void SplitCell(TreeNode node)
	{
		if (node.Dimensions.X > node.Dimensions.Y)
			SplitCellVertical(node);
		else
			SplitCellHorizontal(node);
	}

	private void SplitCellVertical(TreeNode node)
	{
		Vector2I leftDimensions = new(
			node.Dimensions.X / (int)Math.Max(GD.Randi() % _maxPartitionDivision, 2),
			node.Dimensions.Y
		);
		Vector2I rightDimensions = new(
			node.Dimensions.X - leftDimensions.X,
			node.Dimensions.Y
		);
		
		TreeNode leftNode = new(node.Position, leftDimensions, node.Depth + 1);
		
		Vector2I rightPosition = new(node.Position.X + leftDimensions.X, node.Position.Y);
		TreeNode rightNode = new(rightPosition, rightDimensions, node.Depth + 1);
		
		node.Left = leftNode;
		node.Right = rightNode;
	}

	private void SplitCellHorizontal(TreeNode node)
	{
		Vector2I leftDimensions = new(
			node.Dimensions.X,
			node.Dimensions.Y / (int)Math.Max(GD.Randi() % _maxPartitionDivision, 2)
		);
		Vector2I rightDimensions = new(
			node.Dimensions.X,
			node.Dimensions.Y - leftDimensions.Y
		);
		
		TreeNode leftNode = new(node.Position, leftDimensions, node.Depth + 1);
		
		Vector2I rightPosition = new(node.Position.X, node.Position.Y + leftDimensions.Y);
		TreeNode rightNode = new(rightPosition, rightDimensions, node.Depth + 1);
		
		node.Left = leftNode;
		node.Right = rightNode;
	}

	private void FindLeafNodes(TreeNode node)
	{
		if (node == null) return; // base case null
		if (node.Left == null && node.Right == null) _leafNodes.Add(node); // base case is leaf
		
		if (node.Left != null) FindLeafNodes(node.Left); // recursive case left
		if (node.Right != null) FindLeafNodes(node.Right); // recursive case right
	}

	private void Clear()
	{
		GD.Print("Clear Partition Tree");
		_nodes.Clear();
		_leafNodes.Clear();
	}
}


