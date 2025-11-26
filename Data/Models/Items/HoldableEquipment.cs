using Godot;
using System;

[GlobalClass]
public partial class HoldableEquipment : Equipment
{
    [Export] public required PackedScene ItemModel { get; set; }
}
