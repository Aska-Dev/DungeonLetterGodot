using Godot;
using System;

[GlobalClass]
public abstract partial class EquipModifier : Resource
{
    public abstract void Apply(StatsComponent statsComponent);
    public abstract void Remove(StatsComponent statsComponent);
}
