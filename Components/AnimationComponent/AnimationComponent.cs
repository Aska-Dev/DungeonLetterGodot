using Godot;
using System;

[GlobalClass]
public partial class AnimationComponent : Component
{
    [Export] public required NodePath AnimationTreePath { get; set; }

    private AnimationTree animationTree = null!;
    private AnimationNodeStateMachinePlayback stateMachinePlayback = null!;

    public override void _Ready()
    {
        animationTree = GetNode<AnimationTree>(AnimationTreePath);
        stateMachinePlayback = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
    }

    public StringName GetState()
    {
        return stateMachinePlayback.GetCurrentNode();
    }

    public void SetCondition(string condition, bool status) {
        animationTree.Set($"parameters/conditions/{condition}", status);
    }

    public void TravelTo(StringName state) {
        stateMachinePlayback.Travel(state);
    }

    public void Start(StringName state) {
        GD.Print("Starting animation state: ", state);
        stateMachinePlayback.Start(state);
    }
}
