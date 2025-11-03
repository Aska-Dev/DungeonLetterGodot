using DungeonLetter.Common;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public partial class DealTrueDamage : DealDamage
{
    public override void Apply(DamageComponent damageComponent)
    {
        damageComponent.Health.Decrease(DamageAmount);
    }
}