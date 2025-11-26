using Godot;
using System;

[GlobalClass]
public abstract partial class PlayerActionRunner : Node
{
    [Signal] public delegate void ActionCompleteEventHandler();
    public virtual void Complete()
    {
        EmitSignal(SignalName.ActionComplete);
        QueueFree();
    }
}
