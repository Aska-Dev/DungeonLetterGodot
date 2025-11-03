using Godot;
using System;

public partial class Holdable : Equipment
{
    [Export] public required PackedScene ItemModel { get; set; }
}
