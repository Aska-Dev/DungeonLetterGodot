using Godot;
using System;

public enum Stats
{
    MaxHealth,
    Armor,
    Resistance
}

[GlobalClass]
public partial class StatsComponent : Component
{
    [ExportCategory("Dependencies")]
    [Export] public ValueComponent? Health { get; set; } = null;

    [ExportCategory("Stats")]
    [Export]
    public int MaxHealth
    {
        get => Health?.MaxValue ?? 0;
        private set
        {
            if (Health is not null)
            {
                Health.MaxValue = value;
                if(Health.Value > value)
                {
                    Health.Set(value);
                }
            }
        }
    }

    [Export] public int Armor { get; private set; } = 0;
    [Export] public int Resistance { get; private set; } = 0;
    [Export] public int MovementSpeed { get; private set; } = 8;
    [Export ] public int PhysicalAttackBonus { get; private set; } = 0;
    [Export ] public int MagicalAttackBonus { get; private set; } = 0;

    public int GetStat(Stats stat)
    {
        return stat switch
        {
            Stats.MaxHealth => MaxHealth,
            Stats.Armor => Armor,
            Stats.Resistance => Resistance,
            _ => throw new ArgumentOutOfRangeException(nameof(stat), stat, null)
        };
    }

    public void SetStat(Stats stat, int newValue)
    {
        switch (stat)
        {
            case Stats.MaxHealth:
                MaxHealth = newValue;
                break;
            case Stats.Armor:
                Armor = newValue;
                break;
            case Stats.Resistance:
                Resistance = newValue;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
        }
    }
}
