using Godot;
using System;
using System.Linq;
using System.Text;

public partial class ActionChainRunner : PlayerActionRunner
{
    public required ActionChainStep[] Steps { get; set; }
    
    private ActionChainStepData? nextAction = null;
    private ActionChainStepData? currentAction = null;

    private AnimationPlayer animationPlayer = null!;
    private bool isStepInProgress = false;
    private int currentStepIndex = 0;
    private bool isLastStepIndex => currentStepIndex >= Steps.Length -1;


    public override void _Ready()
    {
        if (Steps[0].Actions.Length > 1)
        {
            GD.PrintErr("ActionChainRunner does not support multiple actions in the first step");
            throw new Exception("ActionChainRunner does not support multiple actions in the first step");
        }

        var player = GetTree().GetFirstNodeInGroup("player") as Player;
        animationPlayer = player!.GetNode<AnimationPlayer>("AnimationPlayer");

        RunAction(Steps[0].Actions[0]);
    }

    public override void _Input(InputEvent @event)
    {
        if (isStepInProgress && !isLastStepIndex)
        {
            var triggeredAction = Steps[currentStepIndex+1].Actions.FirstOrDefault(a => @event.IsActionPressed(a.Action.TriggerInputAction));
            if(triggeredAction is not null)
            {
                nextAction = triggeredAction;
            }

            GD.Print($"Next action set to: {nextAction?.Action.GetType().Name ?? "null"}");
        }
    }

    public void Reset()
    {
        if(currentAction is not null)
        {
            animationPlayer.Play(currentAction.ResetAnimationName);
            animationPlayer.AnimationFinished += (_) => Complete();
        }
    }

    private void OnStepFinished()
    {
        // Clean up current action runner
        foreach (var child in GetChildren())
        {
            child.QueueFree();
        }

        // Mark step as completed
        isStepInProgress = false;

        // Check if we are at the last step or no next action was selected
        if (isLastStepIndex || nextAction is null)
        {
            Reset();
            return;
        }

        currentStepIndex++;
        RunAction(nextAction);
        nextAction = null;
    }

    private void RunAction(ActionChainStepData action)
    {
        currentAction = action;

        var runner = currentAction.Action.CreateActionRunner();
        runner.ActionComplete += OnStepFinished;
        AddChild(runner);

        isStepInProgress = true;
    }
}
