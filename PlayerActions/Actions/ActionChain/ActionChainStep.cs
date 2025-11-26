using Godot;
using System;

[GlobalClass]
public partial class ActionChainStep : Resource
{
    [Export] public required ActionChainStepData[] Actions { get; set; }
}
