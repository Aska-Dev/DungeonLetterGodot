using DungeonLetter.Common;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class PlayerActionControllerComponent : Component
{
    public List<ActionData> PlayerActions = [];

    public bool IsPerformingAction = false;

    public override void _Input(InputEvent @event)
    {
        if(IsPerformingAction || UiEventBus.Instance.IsUiOpen)
        {
            return;
        }

        if(@event.IsActionPressed(Inputs.ActionPrimary))
        {
            GD.Print(PlayerActions.Count);

            var triggerAction = GetActionByTrigger(Inputs.ActionPrimary);
            if (triggerAction is not null)
            {
                StartAction(triggerAction);
            }
            return;
        }

        if(@event.IsActionPressed(Inputs.ActionSecondary))
        {
            var triggerAction = GetActionByTrigger(Inputs.ActionSecondary);
            if(triggerAction is not null)
            {
                StartAction(triggerAction);
            }
            return;
        }
    }

    public void StartAction(ActionData data)
    {
        IsPerformingAction = true;
        var runner = data.CreateActionRunner();
        AddChild(runner);

        runner.ActionComplete += OnActionComplete;
    }

    public void OnActionComplete()
    {
        IsPerformingAction = false;
    }

    public ActionData? GetActionByTrigger(string TriggerInputAction)
    {
        var dataByTrigger = PlayerActions.FindAll(action => action.TriggerInputAction == TriggerInputAction);
        var highestPriorityAction = dataByTrigger.FirstOrDefault(action => action.Priority == dataByTrigger.Max(a => a.Priority));

        return highestPriorityAction;
    }

    private static PlayerActionRunner CreateActionRunner(ActionData data)
    {
        return data switch
        {
            InstantActionData instantActionData => new InstantActionRunner()
            {
                AnimationName = instantActionData.AnimationName,
                ActionCompleteEvents = instantActionData.ActionCompleteEvents,
            },
            _ => throw new NotImplementedException($"ActionData type {data.GetType().Name} not implemented in ActionRunnerBuilder."),
        };
    }
}
