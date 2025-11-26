using Godot;
using System;

[GlobalClass]
public partial class InstantActionRunner : PlayerActionRunner
{
    public ActionEventData[] ActionCompleteEvents { get; set; } = [];
    public StringName AnimationName { get; set; } = string.Empty;

    public override void _Ready()
    {
        GD.Print("Play animation " + AnimationName);

        var player = GetTree().GetFirstNodeInGroup("player") as Player;
        var animationPlayer = player!.GetNode<AnimationPlayer>("AnimationPlayer");

        animationPlayer.AnimationFinished += OnAnimationFinished;

        GD.Print("Playing animation: " + AnimationName);
        animationPlayer.Play(AnimationName);
    }
    
    public void OnAnimationFinished(StringName animationName)
    {
        var root = GetTree().Root;

        foreach (var actionEvent in ActionCompleteEvents)
        {
            actionEvent.Execute(root);
        }
        Complete();
    }
}
