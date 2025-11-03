using Godot;
using System;

[GlobalClass]
public partial class AttackCombo : Resource
{
    [Export] public ComboStep[] Steps { get; set; } = [];
    [Export] public string InputActionName { get; set; } = string.Empty;
}
