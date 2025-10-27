using Godot;
using System;

[GlobalClass]
public partial class InteractionRayComponent : Component
{
    [Signal]
    public delegate void OnInteractionComponentChangeEventHandler(InteractionComponent? component);

    [Export]
    public required RayCast3D Ray { get; set; }

    public InteractionComponent? CurrentInteractionComponent { get; private set; }

    public override void _PhysicsProcess(double delta)
    {
        if(Ray.IsColliding() && Ray.GetCollider() is IEntity entity)
        {
            var interactionComponent = entity.Components.Get<InteractionComponent>();
            if(interactionComponent is null)
            {
                return;
            }

            if (CurrentInteractionComponent != interactionComponent)
            {
                if (CurrentInteractionComponent is not null)
                {
                    CurrentInteractionComponent.OnRayHit(false);
                }

                CurrentInteractionComponent = interactionComponent;
                CurrentInteractionComponent.OnRayHit(true);

                EmitSignal(SignalName.OnInteractionComponentChange, CurrentInteractionComponent);
            }
        }
        else if(CurrentInteractionComponent is not null)
        {
            CurrentInteractionComponent.OnRayHit(false);
            CurrentInteractionComponent = null;

            EmitSignal(SignalName.OnInteractionComponentChange, null);
        }
    }

}
