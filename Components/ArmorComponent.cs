using Godot;
using System;

public partial class ArmorComponent : Component
{
    [Export] public required int Armor { get; set; } = 0;
    [Export] public required int Resistance { get; set; } = 0;
}
