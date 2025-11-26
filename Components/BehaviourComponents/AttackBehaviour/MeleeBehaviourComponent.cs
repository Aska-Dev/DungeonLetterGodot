using Godot;
using System;

[GlobalClass]
public partial class MeleeBehaviourComponent : AttackBehaviourComponent
{
    [Signal] public delegate void OnMeleeAttackEventHandler();

    [Export] public AnimationComponent AnimationComponent { get; set; } = null!;
    [Export] public float AttackRange { get; set; } = 2.0f;
    [Export] public AttackModifier[] Attacks { get; set; } = Array.Empty<AttackModifier>();

    public override void _PhysicsProcess(double delta)
    {
        var player = GetTree().GetFirstNodeInGroup("player") as Player;
        var parent = GetParent<Node3D>();
        var distanceToPlayer = parent.GlobalPosition.DistanceTo(player.GlobalPosition);

        if (distanceToPlayer <= AttackRange)
        {
            PerformMeleeAttack();
        }
    }

    public override void OnAttackHit(Node3D body)
    {
        GD.Print("Melee attack hit detected.");

        if (body is Player player)
        {
            var enemy = GetParent<Enemy>();

            var onAttackHitComponent = player.Components.Get<OnAttackHitComponent>();
            if (onAttackHitComponent is not null)
            {
                onAttackHitComponent.OnHit(enemy!, Attacks);
            }
        }
    }

    private void PerformMeleeAttack()
    {
        var currentState = AnimationComponent.GetState();
        
        if(currentState == "idle")
        {
            EmitSignal(SignalName.OnMeleeAttack);
        }
    }
}
