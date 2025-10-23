using DungeonLetter.Common;
using Godot;

public partial class Enemy : CharacterBody3D, ICharacter
{
    [Export]
    public int MaxHealth { get; set; }
	public int Health { get; set; }
	[Export]
	public int Armor { get; set; }
	[Export]
	public int Resistance { get; set; }

	protected AnimationTreeController animator;

	private CollisionShape3D _collider;

	public override void _Ready()
	{
		Health = MaxHealth;

        animator = new AnimationTreeController(GetNode<AnimationTree>("Pivot/Model/AnimationTree"));
        _collider = GetNode<CollisionShape3D>("Collision");
		
	}

	public void SetCollision(bool status)
	{
		_collider.SetDeferred("disabled", !status);
    }

    public virtual void OnHit(ICharacter source, AttackModifier[] attackModifier)
	{
        AttackProcessor.Execute(source, this, attackModifier);
		
		if(Health > 0)
		{
            animator.OneShot("hit");
        }
		else
		{
            Kill();
        }
    }

	public virtual void Kill()
	{
		SetCollision(false);
		animator.SetState("dead");

		animator.AnimTree.AnimationFinished += DestroyAfterDeath;
	}

	public virtual void DestroyAfterDeath(StringName animName)
	{
		QueueFree();
	}
}
