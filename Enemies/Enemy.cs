using DungeonLetter.Common;
using Godot;

public partial class Enemy : CharacterBody3D, IEntity
{
	[Export] public AttackBehaviourComponent AttackBehaviour { get; set; } = null!;

    public Components Components { get; set; } = null!;

	private Node3D? weapon = null;
	
	public override void _Ready()
	{
		Components = new Components(this);

        var rightHand = GetNodeOrNull<BoneAttachment3D>("Pivot/Model/Rig/Skeleton3D/handslot_r");
		if(rightHand is not null)
		{
			GD.Print("Right hand found for enemy: " + Name);
            weapon = rightHand.GetChildOrNull<Node3D>(0);
        }

		if(weapon is not null)
		{
			GD.Print("Enemy equipped with weapon: " + weapon.Name);
            var hitbox = weapon.GetNode<Area3D>("Hitbox");
			hitbox.BodyEntered += AttackBehaviour.OnAttackHit;
        }
    }
}