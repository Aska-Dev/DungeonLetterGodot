using Godot;
using System;

public enum EquipmentTypes
{
    MainHand,
    OffHand,
    Headgear,
    BodyArmor,
    LegArmor,
    Boots,
    Consumable
}

[GlobalClass]
public partial class Equipment : Item
{
    [Export] public virtual required EquipmentTypes Type { get; set; }
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
