using Godot;
using System;

public enum TriggerTypes
{
    OneShot,
    SetState
}

[GlobalClass]
public partial class AnimationTrigger : Node
{
    [Export]
    public string TriggerName { get; set; } = "";

    [Export]
    public TriggerTypes TriggerType { get; set; } = TriggerTypes.OneShot;

    private AnimationComponent animComp = null!;

    public override void _Ready()
    {
        animComp = GetParent<AnimationComponent>();
    }
    
    public void Trigger()
    {
        switch(TriggerType)
        {
            case TriggerTypes.OneShot:
                animComp.OneShot(TriggerName);
                break;
            case TriggerTypes.SetState:
                animComp.SetState(TriggerName);
                break;
            default:
                GD.PrintErr("Unknown TriggerType: " + TriggerType);
                break;
        }
    }

}
