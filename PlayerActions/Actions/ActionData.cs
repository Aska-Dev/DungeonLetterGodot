using DungeonLetter.Common;
using Godot;
using System;

[GlobalClass]
public abstract partial class ActionData : Resource
{
    [Export] public required int Priority { get; set; }

    [Export(PropertyHint.Enum, $"{Inputs.ActionPrimary},{Inputs.ActionSecondary}")]
    public required string TriggerInputAction { get; set; }

    public abstract PlayerActionRunner CreateActionRunner();
}
