using Godot;
using System;

[GlobalClass]
public partial class ActionChainStepData : Resource
{
    [Export] public required ActionData Action { get; set; }
    [Export] public required string ResetAnimationName { get; set; }
}
