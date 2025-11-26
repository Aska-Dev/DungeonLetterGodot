using DungeonLetter.Common;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public partial class Weapon : TriggerableEquipment
{
    [Export] public AttackModifier[] AttackModifiers { get; set; } = [];
}
