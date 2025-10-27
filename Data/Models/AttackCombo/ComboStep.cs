using Godot;
using System;

[GlobalClass]
public partial class ComboStep : Resource
{
    [Export] public string AnimationName { get; set; } = string.Empty;
    [Export] public string ResetAnimationName { get; set; } = string.Empty;
}