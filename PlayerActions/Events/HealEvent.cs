using Godot;
using System;

[GlobalClass]
public partial class HealEvent : ActionEventData
{
    [Export] public required int HealAmount { get; set; }
    public override void Execute(Node root)
    {
        var player = root.GetTree().GetFirstNodeInGroup("player") as Player;
        player!.Heal(HealAmount);
    }
}
