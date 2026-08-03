using System;
using Godot;

namespace LootGoblin;

public partial class StateMachine : Node
{
    [Export] private State _initialState;
    private State _currentState;

    public override void _Ready()
    {
        foreach (Node node in GetChildren())
        {
            State stateNode = (State)node;
            stateNode.Finished += TransitionToState;
        }

        _currentState = _initialState;
        _currentState.Enter("");
    }

    public override void _Process(double delta)
    {
        _currentState.Update(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        _currentState.PhysicsUpdate(delta);
    }

    private void TransitionToState(string statePath)
    {
        if (!HasNode(statePath))
        {
            GD.PushError(Owner.Name + ": Trying to transition to " + statePath + " but it does not exist");
            return;
        }

        StringName previousStatePath = _currentState.Name;
        _currentState.Exit();
        _currentState = GetNode<State>(statePath);
        _currentState.Enter(previousStatePath);
        GD.Print("Entered " + _currentState.Name);
    }
    
}