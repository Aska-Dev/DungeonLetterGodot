using Godot;
using System;

[GlobalClass]
public partial class PlayAnimationEvent : ActionEventData
{
    [Export] public required string AnimationName { get; set; }
    public override void Execute(Node root)
    {
        var player = root.GetTree().GetFirstNodeInGroup("player") as Player;
        var animationPlayer = player!.GetNode<AnimationPlayer>("AnimationPlayer");

        animationPlayer.Play(AnimationName);
    }
}
