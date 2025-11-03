using Godot;
using System;

public partial class GUI : Control
{
    [Export] public CanvasLayer OverlayLayer { get; set; } = null!;

    public ProgressBar HealthBar = null!;
    public Label InteractionMessageLabel = null!;

    public override void _Ready()
    {
        HealthBar = GetNode<ProgressBar>("Healthbar");
        InteractionMessageLabel = GetNode<Label>("InteractionMessageLabel");

        OverlayLayer.AddToGroup("ui.overlay");

        UiEventBus.Instance.ChangeInteractionLabelText += UpdateInteractionMessageLabel;
    }

    public void UpdateInteractionMessageLabel(string text)
    {
        InteractionMessageLabel.Text = text;
    }
}
