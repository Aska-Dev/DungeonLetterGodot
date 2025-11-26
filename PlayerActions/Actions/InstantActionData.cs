using Godot;
using System;

[GlobalClass]
public partial class InstantActionData : ActionData
{
    [Export] public required string AnimationName { get; set; }
    [Export] public required ActionEventData[] ActionCompleteEvents { get; set; }

    public override PlayerActionRunner CreateActionRunner()
    {
       return new InstantActionRunner
       {
           AnimationName = AnimationName,
           ActionCompleteEvents = ActionCompleteEvents
       };
    }
}
