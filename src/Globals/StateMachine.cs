using System;
using Godot;

namespace LootGoblin;

public partial class StateMachine : Node
{
    [Export] private State _initialState;
    public State CurrentState;

    public override void _Ready()
    {
        foreach (Node node in GetChildren())
        {
            State stateNode = (State)node;
            stateNode.Finished += TransitionToState;
        }

        CurrentState = _initialState;
        CurrentState.CallDeferred(State.MethodName.Enter, "");
        // CurrentState.Enter("");
    }

    public override void _Process(double delta)
    {
        CurrentState.Update(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicsUpdate(delta);
    }

    private void TransitionToState(string statePath)
    {
        if (!HasNode(statePath))
        {
            GD.PushError(Owner.Name + ": Trying to transition to " + statePath + " but it does not exist");
            return;
        }

        StringName previousStatePath = CurrentState.Name;
        CurrentState.Exit();
        CurrentState = GetNode<State>(statePath);
        CurrentState.Enter(previousStatePath);
        // GD.Print("Entered " + _currentState.Name);
    }
    
}