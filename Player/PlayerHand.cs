using Godot;
using System;

public partial class PlayerHand : Node3D
{
	[ExportCategory("Dependencies")]
	[Export] public required StatsComponent StatsComponent { get; set; }
    [Export] public required PlayerActionControllerComponent ActionController { get; set; }

    [Export] public HoldableEquipment? HoldingItem { get; set; } = null;

	public void SetItem(HoldableEquipment item)
	{
        // Clear existing children
        ClearHand();

        // Instantiate the item model and add it as a child
        var modelInstance = item.ItemModel.Instantiate<Node3D>();
		HoldingItem = item;

		AddChild(modelInstance);

        // Apply item modifiers to player stats
        item.ApplyModifiers(StatsComponent);
        // Add item actions to action controller
        if (item is TriggerableEquipment triggerable)
		{
            triggerable.AddActions(ActionController);
        }

        // Connect attack hit signal if item is a weapon
		if (item is Weapon weapon)
		{
			var hitbox = modelInstance.GetNode<Area3D>("Hitbox");
			hitbox.BodyEntered += OnAttackHit;
        }
    }

    public void ClearHand()
	{
		if(HoldingItem is not null)
		{
			HoldingItem.RemoveModifiers(StatsComponent);

			if(HoldingItem is TriggerableEquipment triggerable)
			{
				triggerable.RemoveActions(ActionController);
			}
		}

		// Clear existing children
		foreach (Node3D child in GetChildren())
		{
			child.QueueFree();
		}

		HoldingItem = null;
    }

    public void SetItemHitboxStatus(bool status)
	{
		GetChild(0).GetNode<Area3D>("Hitbox").Monitoring = status;
	}

	private void OnAttackHit(Node3D body)
	{
		if (HoldingItem is Weapon weapon && body is IEntity entity)
		{
			var player = GetParent() as Player;

			var onAttackHitComponent = entity.Components.Get<OnAttackHitComponent>();
			if (onAttackHitComponent is not null)
			{
				onAttackHitComponent.OnHit(player!, weapon.AttackModifiers);
			}
		}
	}
}
