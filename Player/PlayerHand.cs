using Godot;
using System;


public partial class PlayerHand : Node3D
{
	[ExportGroup("Dependencies")]
	[Export] public required AnimationPlayer AnimationPlayer { get; set; }
	[Export] public required PlayerAttackComponent PlayerAttackComponent { get; set; }

    [Export]
	public Item? Item { get; set; } = null;

	public void UseItem()
	{
		if (Item is null || Item is Weapon weapon)
		{
			return;
		}

        AnimationPlayer.Play(Item.UseAnimation);
    }

	public void EquipItem(Item item)
	{
		// Clear existing children
		foreach (Node3D child in GetChildren())
		{
			child.QueueFree();
		}
	
		var loadedModel = GD.Load<PackedScene>(item.Model.Path);
		var modelInstance = loadedModel.Instantiate<Node3D>();

		if(item is Weapon weapon)
		{
			PlayerAttackComponent.IsActive = true;
			PlayerAttackComponent.CurrentCombo = weapon.Combo;

            var itemHitbox = modelInstance.GetChild<Area3D>(1);
            itemHitbox.BodyEntered += OnAttackHit;
        }
		else
		{
			PlayerAttackComponent.IsActive = false;
        }

		Item = item;
		AddChild(modelInstance);
	}

	public void OnAttackHit(Node3D body)
	{
		if (Item is Weapon weapon && body is IEntity entity)
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
