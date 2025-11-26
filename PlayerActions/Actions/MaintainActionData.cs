using Godot;
using System;

[GlobalClass]
public partial class MaintainActionData : ActionData
{
    [Export] public required string StartAnimationName { get; set; }
    [Export] public required string EndAnimationName { get; set; }

    public override PlayerActionRunner CreateActionRunner()
    {
         return new MaintainActionRunner
         {
            TriggerInputAction = TriggerInputAction,
            StartAnimationName = StartAnimationName,
            EndAnimationName = EndAnimationName
         };
    }
}
