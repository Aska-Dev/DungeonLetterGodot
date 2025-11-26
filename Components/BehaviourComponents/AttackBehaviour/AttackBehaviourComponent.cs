using Godot;
using System;

public abstract partial class AttackBehaviourComponent : Component
{
    public abstract void OnAttackHit(Node3D body);
}
