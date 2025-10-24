using DungeonLetter.Common;
using Godot;
using System;

public partial class AttackHitEventArgs : RefCounted
{
    public required IEntity Source { get; set; }
    public required AttackModifier[] Modifiers { get; set; }
}

[GlobalClass]
public partial class OnAttackHitComponent : Component
{
    [Signal]
    public delegate void OnAttackHitEventHandler(AttackHitEventArgs args);

    public void OnHit(IEntity source, AttackModifier[] modifiers)
    {
        var args = new AttackHitEventArgs
        {
            Source = source,
            Modifiers = modifiers
        };

        EmitSignal(SignalName.OnAttackHit, args);
    }
}
