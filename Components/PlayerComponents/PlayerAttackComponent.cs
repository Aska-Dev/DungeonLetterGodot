using Godot;
using System;

[GlobalClass]
public partial class PlayerAttackComponent : Component
{
    [Export]
    public required AnimationPlayer AttackAnimationPlayer { get; set; }

    [Export]
    public bool IsActive { get; set; } = true;

    public required AttackCombo CurrentCombo { get; set; }

    private int currentStepIndex = -1;
    private bool queueNextStep = false;

    private bool lastStep => currentStepIndex == CurrentCombo.Steps.Length - 1;
    private bool isInCombo => currentStepIndex >= 0;
    private bool isPlayingAnim = false;
    private bool isResetting = false;

    public override void _Input(InputEvent @event)
    {
        if (!IsActive || CurrentCombo?.Steps == null || CurrentCombo.Steps.Length == 0)
        {
            return;
        }

        if (@event.IsActionPressed(CurrentCombo.InputActionName))
        {
            GD.Print("Attack input received");
            if (isPlayingAnim)
            {
                GD.Print("Animation is currently playing");
                if (!lastStep && !isResetting)
                {
                    queueNextStep = true;
                }

                return;
            }

            PlayAnimation();
        }
    }

    public void OnAttackAnimationFinished(StringName animationName)
    {
        if (!IsActive || !isPlayingAnim)
        {
            return;
        }

        if (isResetting)
        {
            EndReset();
            return;
        }

        if (queueNextStep && !lastStep)
        {
            queueNextStep = false;
            PlayAnimation();
        }
        else
        {
            StartReset();
        }
    }

    private void PlayAnimation()
    {
        currentStepIndex++;

        AttackAnimationPlayer.Play(CurrentCombo.Steps[currentStepIndex].AnimationName);
        isPlayingAnim = true;
    }

    private void StartReset()
    {
        isPlayingAnim = true;
        isResetting = true;

        AttackAnimationPlayer.Play(CurrentCombo.Steps[currentStepIndex].ResetAnimationName);
    }

    private void EndReset()
    {
        isPlayingAnim = false;
        isResetting = false;

        currentStepIndex = -1;
    }
}
