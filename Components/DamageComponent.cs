using DungeonLetter.Common;
using Godot;
using System;
using System.Linq;

[GlobalClass]
public partial class DamageComponent : Component
{
    [ExportGroup("Dependencies")]
    [Export] public required ValueComponent Health { get; set; }
    [Export] public StatsComponent? Stats { get; set; } = null;


    [Signal]
    public delegate void OnDamageTakenEventHandler();

    public void TakeDamage(DealDamage[] damageModifiers)
    {
        foreach(var dmg in damageModifiers)
        {
            dmg.Apply(this);
        }

        EmitSignal(SignalName.OnDamageTaken);
    }

    public void OnHit(AttackHitEventArgs args)
    {
        var damageModifiers = args.Modifiers.OfType<DealDamage>().ToArray();
        
        if(damageModifiers.Length > 0)
        {
            TakeDamage(damageModifiers);
        }
    }
}
