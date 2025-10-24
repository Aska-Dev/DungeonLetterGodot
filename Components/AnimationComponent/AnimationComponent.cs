using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

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
