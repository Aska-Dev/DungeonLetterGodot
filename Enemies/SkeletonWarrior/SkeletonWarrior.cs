using Godot;
using System;

public partial class SkeletonWarrior : CharacterBody3D, IEntity
{
    public Components Components { get; set; } = null!;
    
    public override void _Ready()
    {
        Components = new Components(this);
    }
}
