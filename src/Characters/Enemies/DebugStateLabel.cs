using Godot;
using System;

namespace LootGoblin;

public partial class DebugStateLabel : Label
{
	[Export] private StateMachine _stateMachine;

	public override void _PhysicsProcess(double delta)
	{
		Text = _stateMachine.CurrentState.StateName;
	}
}
