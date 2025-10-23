using Godot;
using System;

[GlobalClass]
public partial class CollisionComponent : Component
{
    [Export]
    public CollisionShape3D CollisionShape {  get; set; }

    public void EnableCollision(bool value)
    {
        CollisionShape.SetDeferred("disabled", !value);
    }
}
