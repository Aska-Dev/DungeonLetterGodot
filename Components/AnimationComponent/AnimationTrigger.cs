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
        GD.Print(Name);

        switch (TriggerType)
        {
            case TriggerTypes.OneShot:
                AnimationComponent.OneShot(TriggerName);
                break;
            case TriggerTypes.SetState:
                AnimationComponent.SetState(TriggerName);
                break;
            case TriggerTypes.CancelOneShot:
                AnimationComponent.CancelOneShot(TriggerName);
                break;
            default:
                GD.PrintErr("Unknown TriggerType: " + TriggerType);
                break;
        }
    }

}
