using Godot;
using System;

[GlobalClass]
public partial class AnimationChain : Resource
{
    [Export] public AnimationChainStep[] Steps { get; set; } = [];
    [Export] public string InputActionName { get; set; } = string.Empty;
}
