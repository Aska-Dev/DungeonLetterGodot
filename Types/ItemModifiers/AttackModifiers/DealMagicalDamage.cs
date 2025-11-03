using DungeonLetter.Common;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public partial class DealMagicalDamage : DealDamage
{
    public override void Apply(DamageComponent damageComponent)
    {
        var dmg = DamageAmount;

        if (damageComponent.Stats is not null)
        {
            dmg = Mathf.Max(0, DamageAmount - damageComponent.Stats.Resistance);
        }

        damageComponent.Health.Decrease(dmg);
    }
}