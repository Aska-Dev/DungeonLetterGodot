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
    public override void Apply(DamageComponent damageComponent)
    {
        var reducedDmg = Mathf.Max(0, DamageAmount - damageComponent.Armor);
        damageComponent.Health.Decrease(reducedDmg);
    }
}
