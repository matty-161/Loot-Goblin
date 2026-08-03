using System;
using Godot;

namespace LootGoblin;


public abstract partial class State : Node
{
    public Action<string> Finished;

    public virtual void Update(double delta)
    {
        
    }

    public virtual void PhysicsUpdate(double delta)
    {
        
    }

    public virtual void Enter(string previousStatePath)
    {
        
    }

    public virtual void Exit()
    {
        
    }

}