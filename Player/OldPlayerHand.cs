using Godot;
using System;

public partial class PlayerHand : Node3D
{
	[ExportCategory("Dependencies")]
	[Export] public required AnimationPlayer AnimationPlayer { get; set; }
	[Export] public required PlayerAttackComponent PlayerAttackComponent { get; set; }

	[ExportCategory("State")]
    [Export] public Item? HoldingItem { get; set; } = null;

	public void EquipItem(Holdable item)
	{
		GD.Print($"Equipping item: {item.Name}");

        // Clear existing children
        ClearHand();

		var modelInstance = item.ItemModel.Instantiate<Node3D>();

		if(item is Weapon weapon)
		{
			PlayerAttackComponent.IsActive = true;
			PlayerAttackComponent.CurrentCombo = weapon.Combo;
			PlayerAttackComponent.HardReset();

            var itemHitbox = modelInstance.GetChild<Area3D>(1);
            itemHitbox.BodyEntered += OnAttackHit;
        }

        HoldingItem = item;
		AddChild(modelInstance);
	}

	public void ClearHand()
	{
		// Clear existing children
		foreach (Node3D child in GetChildren())
		{
			child.QueueFree();
		}

		HoldingItem = null;
		PlayerAttackComponent.IsActive = false;
    }

	public void OnAttackHit(Node3D body)
	{
		if (HoldingItem is Weapon weapon && body is IEntity entity)
		{
			var player = GetParent() as Player;
			
			var onAttackHitComponent = entity.Components.Get<OnAttackHitComponent>();
			if(onAttackHitComponent is not null)
			{
				onAttackHitComponent.OnHit(player!, weapon.AttackModifiers);
            }
        }
	}

	public void SetWeaponHitboxStatus(bool status)
	{
		GetChild(0).GetNode<Area3D>("Hitbox").Monitoring = status;
	}
}
