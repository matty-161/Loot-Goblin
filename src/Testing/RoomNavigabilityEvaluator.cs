using Godot;
using System;

namespace LootGoblin;

public partial class RoomNavigabilityEvaluator : Node2D
{
	[Export] private NavigationAgent2D _agent;

	private Vector2 _target;
	public Vector2 Target
	{
		get => _target;
		set
		{
			_agent.SetTargetPosition(value);
			_target = value;
		}
	}

	public bool CanReachTarget()
	{
		return _agent.IsTargetReachable();
	}

	public override void _PhysicsProcess(double delta)
	{
		_agent.GetNextPathPosition();
	}
}
