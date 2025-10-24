using Godot;
using System;

[GlobalClass]
public partial class AnimationTriggerCollection : Node
{
	[Export]
	public required AnimationTrigger[] Triggers { get; set; } = [];

	public void TriggerAll()
	{
        foreach (var trigger in Triggers)
		{
			trigger.Trigger();
        }
    }
}
