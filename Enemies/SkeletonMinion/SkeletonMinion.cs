using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class SkeletonMinion : CharacterBody3D, IEntity
{
    public Components Components { get; set; } = null!;

    public override void _Ready()
    {
        Components = new Components(this);
    }
}