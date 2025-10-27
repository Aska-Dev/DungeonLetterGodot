using DungeonLetter.Common;
using Godot;
using System;


public partial class Player : CharacterBody3D, IEntity
{
    // CHARACTER STATS
    [Export]
    public int MaxHealth { get; set; }
	[Export]
	public int Armor { get; set; }
	[Export]
	public int Resistance { get; set; }

	private int _health;
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            guiController.HealthBar.Value = value;
            _health = value;
        }
    }

    // MOVEMENT STATS
    [Export]
	public float WalkingSpeed { get; set; } = 8f;
	[Export]
	public float SprintingSpeed { get; set; } = 14f;
	[Export]
	public float JumpVelocity { get; set; } = 3.5f;

	public Components Components { get; set; } = null!;

    // INJECTIONS
    private ResourcePreloader itemDb;
	private Node3D pivot;
	private Camera3D camera;
	private AnimationPlayer animationPlayer;
	private PlayerHand mainHand;
	private PlayerUi guiController;
	private RayCast3D interactionRay;

	public override void _Ready()
	{
        Components = new Components(this);

        itemDb = GetNode<ResourcePreloader>("/root/Database/Items");

		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		mainHand = GetNode<PlayerHand>("Pivot/PlayerCamera/MainHandPivot");
		pivot = GetNode<Node3D>("Pivot");
		camera = GetNode<Camera3D>("Pivot/PlayerCamera");
        guiController = GetNode<PlayerUi>("PlayerGui");
		interactionRay = GetNode<RayCast3D>("Pivot/PlayerCamera/InteractionRay");

        Health = MaxHealth;
        guiController.HealthBar.MaxValue = Health;
        guiController.HealthBar.Value = Health;

        var startWeapon = itemDb.GetResource("Weapon_RustySword") as Weapon;
		mainHand.EquipItem(startWeapon);
	}

	public override void _Input(InputEvent @event)
	{
		// Handle camera control
		if (@event is InputEventMouseMotion mouseMotion)
		{
			pivot.RotateY(-mouseMotion.Relative.X * 0.002f);
			camera.RotateX(-mouseMotion.Relative.Y * 0.002f);

			var clampedRotation = camera.Rotation;
			clampedRotation.X = Mathf.Clamp(camera.Rotation.X, -(Mathf.Pi / 4), Mathf.Pi / 4);
			camera.Rotation = clampedRotation;
		}

		// Handle action input
		if(@event.IsActionPressed(Inputs.ActionFirst))
		{
			if(mainHand.Item is null)
			{
				return;
			}

			mainHand.UseItem();
		}	

		// Switch weapons
		if(@event.IsActionPressed(Inputs.ActionSwitchWeapon))
		{
			if(mainHand.Item.Name == "Rusty Sword")
			{
				var newItem = itemDb.GetResource("Weapon_LumberjackAxe") as Item;
				mainHand.EquipItem(newItem);
			}
			else
			{
				var newItem = itemDb.GetResource("Weapon_RustySword") as Item;
				mainHand.EquipItem(newItem);
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		HandlePlayerMovement(delta);
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
