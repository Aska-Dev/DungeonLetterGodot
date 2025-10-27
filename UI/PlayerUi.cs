using Godot;
using System;

public partial class PlayerUi : Control
{
    public ProgressBar HealthBar = null!;
    public Label InteractionMessageLabel = null!;

    public override void _Ready()
    {
        HealthBar = GetNode<ProgressBar>("Healthbar");
        InteractionMessageLabel = GetNode<Label>("InteractionMessageLabel");

        UiEventBus.Instance.ChangeInteractionLabelText += UpdateInteractionMessageLabel;
    }

    public void UpdateInteractionMessageLabel(string text)
    {
        InteractionMessageLabel.Text = text;
    }
}
