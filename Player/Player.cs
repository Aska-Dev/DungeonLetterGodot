using DungeonLetter.Common;
using Godot;
using System;


public partial class Player : CharacterBody3D, IEntity
{
    // MOVEMENT STATS
    [Export]
	public float WalkingSpeed { get; set; } = 8f;
	[Export]
	public float SprintingSpeed { get; set; } = 14f;
	[Export]
	public float JumpVelocity { get; set; } = 3.5f;

	public Components Components { get; set; } = null!;

	public bool IsInputEnabled { get; set; } = true;

    // INJECTIONS
    private ResourcePreloader itemDb;
	private Node3D pivot;
	private Camera3D camera;

	public override void _Ready()
	{
        Components = new Components(this);

        itemDb = GetNode<ResourcePreloader>("/root/Database/Items");
		pivot = GetNode<Node3D>("Pivot");
		camera = GetNode<Camera3D>("Pivot/PlayerCamera");

		UiEventBus.Instance.OnUiOpen += OnUiOpened;
		UiEventBus.Instance.OnUiClose += OnUiClosed;
    }

	public override void _Input(InputEvent @event)
	{
		if(!IsInputEnabled)
		{
			return;
		}

		// Handle camera control
		if (@event is InputEventMouseMotion mouseMotion)
		{
			pivot.RotateY(-mouseMotion.Relative.X * 0.002f);
			camera.RotateX(-mouseMotion.Relative.Y * 0.002f);

			var clampedRotation = camera.Rotation;
			clampedRotation.X = Mathf.Clamp(camera.Rotation.X, -(Mathf.Pi / 4), Mathf.Pi / 4);
			camera.Rotation = clampedRotation;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if(!IsInputEnabled)
		{
			return;
        }

		HandlePlayerMovement(delta);
	}

	private void OnUiOpened(UiTriggerEventArgs args)
	{
		IsInputEnabled = false;
    }

	private void OnUiClosed()
	{
		IsInputEnabled = true;
    }

    private void HandlePlayerMovement(double delta)
	{
		var velocity = Velocity;
		var speed = WalkingSpeed;

		velocity = HandleGravity(velocity, delta);

		// Handle jump
		if(Input.IsActionJustPressed(Inputs.MoveJump) && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Handle floor movement
		var inputDirection = Input.GetVector(Inputs.MoveLeft, Inputs.MoveRight, Inputs.MoveForward, Inputs.MoveBack);
		var direction = (pivot.GlobalTransform.Basis * new Vector3(inputDirection.X, 0, inputDirection.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private Vector3 HandleGravity(Vector3 velocity, double delta)
	{
		if(!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		return velocity;
	}
}
