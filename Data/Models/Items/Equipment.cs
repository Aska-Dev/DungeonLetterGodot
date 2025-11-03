using Godot;
using System;

[GlobalClass]
public partial class Equipment : Item
{
    [Export] public EquipModifier[] EquipModifiers { get; set; } = [];

    public void ApplyModifiers(StatsComponent stats)
    {
        foreach (var modifier in EquipModifiers)
        {
            modifier.Apply(stats);
        }
    }

    public void RemoveModifiers(StatsComponent stats)
    {
        foreach (var modifier in EquipModifiers)
        {
            modifier.Remove(stats);
        }
    }
}
