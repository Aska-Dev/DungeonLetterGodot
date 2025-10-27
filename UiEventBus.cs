using Godot;
using System;

public partial class UiEventBus : Node
{
    public static UiEventBus Instance { get; private set; }

    [Signal]
    public delegate void ChangeInteractionLabelTextEventHandler(string label);
    public void ShowInteractionText(string text) => EmitSignal(SignalName.ChangeInteractionLabelText, text);
    public void ClearInteractionText() => EmitSignal(SignalName.ChangeInteractionLabelText, string.Empty);

    public override void _Ready()
    {
        Instance = this;
    }   
}
