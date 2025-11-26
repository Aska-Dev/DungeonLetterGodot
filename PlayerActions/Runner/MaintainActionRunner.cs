using Godot;
using System;

public partial class MaintainActionRunner : PlayerActionRunner
{
    public string TriggerInputAction { get; set; } = string.Empty;
    public StringName StartAnimationName { get; set; } = string.Empty;
    public StringName EndAnimationName { get; set; } = string.Empty;

    private AnimationPlayer animationPlayer = null!;
    private bool isActive = true;

    public override void _Ready()
    {
        var player = GetTree().GetFirstNodeInGroup("player") as Player;
        animationPlayer = player!.GetNode<AnimationPlayer>("AnimationPlayer");

        animationPlayer.Play(StartAnimationName);
    }

    public override void _Input(InputEvent @event)
    {
        if(isActive && @event.IsActionReleased(TriggerInputAction))
        {
            isActive = false;
            PlayEndAnimation();
        }
    }

    private void PlayEndAnimation()
    {
        animationPlayer.Play(EndAnimationName);
        Complete();
    }
}
