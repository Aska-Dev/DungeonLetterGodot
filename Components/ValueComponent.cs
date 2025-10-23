using Godot;
using System;

[GlobalClass]
public partial class ValueComponent : Component
{
    [Export]
    public required int MaxValue { get; set; } = 1;
    [Export]
    public required int MinValue { get; set; } = 0;

    public int Value { get; private set; }

    public override void _Ready()
    {
        Value = MaxValue;
    }

    public void Set(int value)
    {
        Value = value;
    }

    public void Increase(int value)
    {
        Value = Mathf.Min(Value + value, MaxValue);
    }

    public void Decrease(int value)
    {
        Value = Mathf.Max(Value - value, MinValue);
    }
}
