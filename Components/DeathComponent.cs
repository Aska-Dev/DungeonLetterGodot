using Godot;
using System;

[GlobalClass]
public partial class DeathComponent : Component
{
    [Signal]
    public delegate void OnDeathEventHandler();

    [Export]
    public required ValueComponent Health { get; set; }

    [Export]
    public required float DeathDelay { get; set; } = 3.0f;

    public bool AlternateDeathHandler { get; set; } = false;

    private float timer = 0f;
    private bool runDeathTimer = false;

    public override void _Ready()
    {
        Health.OnValueChanged += OnHealthChanged;
    }

    public override void _Process(double delta)
    {
        if (runDeathTimer)
        {
            timer += (float)delta;
            if (timer >= DeathDelay)
            {
                GetParent().QueueFree();
            }
        }
    }

    public void OnHealthChanged(ValueEventArgs args)
    {
        if(args.NewValue <= 0)
        {
            TriggerDeath();
        }
    }

    public void Kill()
    {
        Health.Set(0);
    }

    public void TriggerDeath()
    {
        if (!AlternateDeathHandler)
        {
            runDeathTimer = true;
        }
        
        EmitSignal(SignalName.OnDeath);
    }

}
