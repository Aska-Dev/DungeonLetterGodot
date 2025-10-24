using Godot;
using System;

[GlobalClass]
public partial class ResurrectionComponent : Component
{
    [Signal]
    public delegate void OnResurrectEventHandler();

    [ExportGroup("Components")]
    [Export]
    public required ValueComponent HealthComp { get; set; }
    [Export]
    public required DeathComponent DeathComp { get; set; }
    [Export]
    public required CollisionComponent CollisionComp { get; set; }

    [ExportGroup("Settings")]
    [Export]
    public required int ResurrectionAmount { get; set; } = 1;
    [Export]
    public required float ResurrectDelay { get; set; } = 3.0f;
    [Export]
    public required float InvulnerabilityDuration { get; set; } = 2.0f;

    private int resurrectionAmout = 0;
    private float timer = 0f;
    private bool isWaitingForResurrect = false;
    private bool isResurrecting = false;

    public override void _Ready()
    {
        DeathComp.AlternateDeathHandler = true;
        DeathComp.OnDeath += Resurrect;
    }

    public override void _Process(double delta)
    {
        if (isWaitingForResurrect || isResurrecting)
        {
            timer += (float)delta;
        }

        if(isWaitingForResurrect && timer >= ResurrectDelay)
        {
            isWaitingForResurrect = false;
            isResurrecting = true;
            timer = 0f;

            EmitSignal(SignalName.OnResurrect);
        }

        if(isResurrecting && timer >= InvulnerabilityDuration)
        {
            resurrectionAmout++;
            if(resurrectionAmout >= ResurrectionAmount)
            {
                DeathComp.OnDeath -= Resurrect;
                DeathComp.AlternateDeathHandler = false;
            }

            isResurrecting = false;
            CollisionComp.EnableCollision(true);
        }
    }

    public void Resurrect()
    {
        isWaitingForResurrect = true;
    }
}
