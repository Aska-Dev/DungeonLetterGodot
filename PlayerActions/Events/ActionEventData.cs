using Godot;
using System;

[GlobalClass]
public abstract partial class ActionEventData : Resource
{
    public abstract void Execute(Node root);
}
