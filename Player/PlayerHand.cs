using Godot;
using System;


public partial class PlayerHand : Node3D
{
	[Export]
	public Item? Item { get; set; } = null;

	private AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("../../../AnimationPlayer");
	}

	public void UseItem()
	{
		animationPlayer.Play(Item.UseAnimation);
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

		var itemHitbox = modelInstance.GetChild<Area3D>(1);
		itemHitbox.BodyEntered += OnAttackHit;

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
