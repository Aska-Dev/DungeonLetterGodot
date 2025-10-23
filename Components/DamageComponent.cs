using Godot;
using System;

[GlobalClass]
public partial class DamageComponent : Component
{
    [Export]
    public ValueComponent Health { get; set; }

    public void TakeDamage(int damage)
    {
        Health.Decrease(damage);
    }

}
