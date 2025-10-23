using DungeonLetter.Common;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public partial class DealPhysicalDamage : DealDamage
{
    public override void Apply(AttackContext context)
    {
        var reducedDmg = Mathf.Max(0, DamageAmount - context.Target.Armor);
        context.Target.Health -= reducedDmg;
    }
}
