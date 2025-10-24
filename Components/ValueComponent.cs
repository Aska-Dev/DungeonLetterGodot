using Godot;
using System;

public partial class ValueEventArgs : RefCounted
{
    public required int NewValue { get; set; }
}

[GlobalClass]
public partial class ValueComponent : Component
{
    [Export]
    public required int MaxValue { get; set; } = 1;
    [Export]
    public required int MinValue { get; set; } = 0;

    [Signal]
    public delegate void OnValueChangedEventHandler(ValueEventArgs args);

    public int Value { get; private set; }

    public override void _Ready()
    {
        Value = MaxValue;
    }

    public void Set(int value)
    {
        Value = value;
        EmitSignal(SignalName.OnValueChanged, new ValueEventArgs() { NewValue = Value });
    }

    public void Increase(int value)
    {
        Value = Mathf.Min(Value + value, MaxValue);
        EmitSignal(SignalName.OnValueChanged, new ValueEventArgs() { NewValue = Value });
    }

    public void Decrease(int value)
    {
        Value = Mathf.Max(Value - value, MinValue);
        EmitSignal(SignalName.OnValueChanged, new ValueEventArgs() { NewValue = Value });
    }
}
