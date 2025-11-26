using Godot;
using System;

public partial class RustyScimitar : Node3D
{
	[Export] public bool IsEnemyWeapon { get; set; } = false;

	public override void _Ready()
	{
		if (IsEnemyWeapon)
		{
			var hitbox = GetNode<Area3D>("Hitbox");
			hitbox.CollisionLayer = 4; // EnemyWeapon layer
			hitbox.CollisionMask = 1;  // Player layer
        }
    }

    public void SetWeaponHitbox(bool status)
    {
        var hitbox = GetNode<Area3D>("Hitbox");
        hitbox.Monitoring = status;
    }
}
