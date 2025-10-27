using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class AnimationTriggerEventArgs : RefCounted
{
    public TriggerTypes TriggerType { get; set; }
    public string TriggerName { get; set; } = string.Empty;
}

[GlobalClass]
public partial class AnimationComponent : Component
{
    [Export]
    public required NodePath AnimationTreePath { get; set; }

    private AnimationTree animationTree = null!;

    public override void _Ready()
    {
        animationTree = GetNode<AnimationTree>(AnimationTreePath);
    }

    public void Trigger(AnimationTriggerEventArgs args)
    {
        GD.Print($"AnimationComponent Trigger called with TriggerType: {args.TriggerType}, TriggerName: {args.TriggerName}");

        switch (args.TriggerType)
        {
            case TriggerTypes.OneShot:
                OneShot(args.TriggerName);
                break;
            case TriggerTypes.CancelOneShot:
                CancelOneShot(args.TriggerName);
                break;
            case TriggerTypes.SetState:
                SetState(args.TriggerName);
                break;
            default:
                GD.PrintErr("Unknown TriggerType: " + args.TriggerName);
                break;
        }
    }

    public void OneShot(string nodeName)
    {
        animationTree.Set($"parameters/{nodeName}/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
    }

    public void CancelOneShot(string nodeName)
    {
        animationTree.Set($"parameters/{nodeName}/request", (int)AnimationNodeOneShot.OneShotRequest.Abort);
    }

    public void SetState(string state)
    {
        animationTree.Set("parameters/state/transition_request", state);
    }

    public bool Status(string nodeName)
    {
        var result = (bool)animationTree.Get($"parameters/{nodeName}/active");
        return result;
    }
}
