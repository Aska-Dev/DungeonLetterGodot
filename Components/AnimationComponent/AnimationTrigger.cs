using Godot;
using System;

public enum TriggerTypes
{
    OneShot,
    CancelOneShot,
    SetState
}

[GlobalClass]
public partial class AnimationTrigger : Node
{
    [Export]
    public required AnimationComponent AnimationComponent { get; set; }

    [Export]
    public string TriggerName { get; set; } = "";
    [Export]
    public TriggerTypes TriggerType { get; set; } = TriggerTypes.OneShot;

    public void Trigger()
    {
        var args = new AnimationTriggerEventArgs
        {
            TriggerName = TriggerName,
            TriggerType = TriggerType
        };
        AnimationComponent.Trigger(args);
    }
}

