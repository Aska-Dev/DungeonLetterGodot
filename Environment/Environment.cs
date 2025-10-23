using Godot;
using System;

public partial class Environment : StaticBody3D
{

	protected AnimationTreeController animator;

	private CollisionShape3D collider;

	public override void _Ready()
	{
		animator = new AnimationTreeController(GetNode<AnimationTree>("AnimationTree"));

		collider = GetNode<CollisionShape3D>("Collider");
	}

	public void SetCollission(bool value)
	{
        collider.SetDeferred("disabled", !value);
    }
}
