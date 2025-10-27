using Godot;
using System;

public partial class FreezeInputComponent : Component
{
    [Signal] public delegate void OnChangeFreezeEventHandler(bool isFrozen);

    [Export]
    public override bool IsActive { get; set; } = false;
    [Export]
    public Component[] Components { get; private set; } = [];

    public void Freeze()
    {
        if(IsActive)
        {
            return;
        }

        IsActive = true;
        EmitSignal(SignalName.OnChangeFreeze, true);

        foreach(var comp in Components)
        {
            comp.ProcessMode = ProcessModeEnum.Disabled;
        }
    }

    public void Unfreeze()
    {
        if(!IsActive)
        {
            return;
        }

        IsActive = false;
        EmitSignal(SignalName.OnChangeFreeze, false);

        foreach(var comp in Components)
        {
            comp.ProcessMode = ProcessModeEnum.Inherit;
        }
    }
}
