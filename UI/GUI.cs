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
        UiEventBus.Instance.OnHealthBarUpdate += UpdateHealthBar;
    }

    private void UpdateInteractionMessageLabel(string text)
    {
        InteractionMessageLabel.Text = text;
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        HealthBar.MaxValue = maxHealth;
        HealthBar.Value = currentHealth;
    }
}
