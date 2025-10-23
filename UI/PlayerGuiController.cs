using Godot;
using System;

public partial class PlayerGuiController : Control
{
    public ProgressBar HealthBar;

    public override void _Ready()
    {
        HealthBar = GetNode<ProgressBar>("Healthbar");
    }
}
