using Godot;
using System;

[GlobalClass]
public partial class ActionChainData : ActionData
{
    [Export] public required ActionChainStep[] Steps { get; set; }

    public override PlayerActionRunner CreateActionRunner()
    {
        GD.Print("Creating ActionChainRunner with " + Steps.Length + " steps.");

        return new ActionChainRunner
        {
            Steps = Steps
        };
    }
}
