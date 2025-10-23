using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class SkeletonMinion : Enemy
{
    [Export]
    public bool CanResurrect { get; set; } = false;
	[Export]
    public float ResurrectionTimer { get; set; } = 3.0f;

    private float timer = 0f;
    private bool isRessurecting = false;

    public override void _Process(double delta)
    {
        if(isRessurecting)
        {
            timer += (float)delta;

            if (timer >= ResurrectionTimer)
            {
                animator.CancelOneShot("hit");
                animator.OneShot("resurrection");
                animator.SetState("alive");

                animator.AnimTree.AnimationFinished += Ressurect;

                isRessurecting = false;
            }
        }
    }

    public override void Kill()
    {
        SetCollision(false);
        animator.SetState("dead");

        if (CanResurrect)
        {
            isRessurecting = true;
            GD.Print("Resurrection");
        }
        else
        {
            animator.AnimTree.AnimationFinished += DestroyAfterDeath;
        }
    }

    private void Ressurect(StringName animName)
    {
        if(animName != "Death_C_Skeletons_Resurrect")
        {
            return;
        }

        SetCollision(true);

        isRessurecting = false;
        timer = 0f;

        Health = MaxHealth;

        animator.AnimTree.AnimationFinished -= Ressurect;   
    }

}
