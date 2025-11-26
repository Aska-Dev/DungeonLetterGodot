using Godot;
using System;

[GlobalClass]
public partial class TriggerableEquipment : HoldableEquipment
{
    [Export] public required ActionData[] Actions { get; set; } = [];

    public void AddActions(PlayerActionControllerComponent actionController)
    {
        foreach (var action in Actions)
        {
            actionController.PlayerActions.Add(action);
            GD.Print($"Added action {action.GetType().Name} to player action controller.");
        }
    }

    public void RemoveActions(PlayerActionControllerComponent actionController)
    {
        foreach (var action in Actions)
        {
            actionController.PlayerActions.Remove(action);
        }
    }
}
