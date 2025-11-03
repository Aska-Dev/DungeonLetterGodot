using Godot;
using System;

public enum ModifyOperation
{
    Add,
    Subtract,
}

public partial class ModifyStatModifier : EquipModifier
{
    [Export] public Stats Stat { get; set; }
    [Export] public ModifyOperation Operation { get; set; } = ModifyOperation.Add;
    [Export] public int Amount { get; set; }

    public override void Apply(StatsComponent statsComponent)
    {
        if (Operation == ModifyOperation.Add)
        {
            var newValue = statsComponent.GetStat(Stat) + Amount;
            statsComponent.SetStat(Stat, newValue);
        }
        else if (Operation == ModifyOperation.Subtract)
        {
            var newValue = statsComponent.GetStat(Stat) - Amount;
            statsComponent.SetStat(Stat, newValue);
        }
    }

    public override void Remove(StatsComponent statsComponent)
    {
        if (Operation == ModifyOperation.Add)
        {
            var newValue = statsComponent.GetStat(Stat) - Amount;
            statsComponent.SetStat(Stat, newValue);
        }
        else if (Operation == ModifyOperation.Subtract)
        {
            var newValue = statsComponent.GetStat(Stat) + Amount;
            statsComponent.SetStat(Stat, newValue);
        }
    }
}
