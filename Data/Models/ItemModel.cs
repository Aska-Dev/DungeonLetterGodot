using Godot;
using System;

[GlobalClass]
public partial class ItemModel : Resource
{
    [Export]
    public string Path { get; set; }
}