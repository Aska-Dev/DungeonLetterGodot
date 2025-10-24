using DungeonLetter.Common;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract partial class DealDamage : AttackModifier
{
    [Export]
    public int DamageAmount { get; set; }

    public abstract void Apply(DamageComponent damageComponent);
}