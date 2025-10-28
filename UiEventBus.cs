using Godot;
using System;

public enum UserInterfaces
{
    PlayerInventory,
    ContainerInventory,
}

public partial class UiTriggerEventArgs : RefCounted
{
    public required UserInterfaces UserInterface { get; set; }
    public Component? UiComponent { get; set; }
}

public partial class UiEventBus : Node
{
    public static UiEventBus Instance { get; private set; } = null!;

    public override void _Ready()
    {
        Instance = this;
    }

    public bool IsUiOpen { get; set; } = false;

    /// INTERACTION LABEL 
    [Signal]
    public delegate void ChangeInteractionLabelTextEventHandler(string label);
    public void ShowInteractionText(string text) => EmitSignal(SignalName.ChangeInteractionLabelText, text);
    public void ClearInteractionText() => EmitSignal(SignalName.ChangeInteractionLabelText, string.Empty);

    /// UI 
    [Signal] public delegate void OnUiCloseEventHandler();
    [Signal] public delegate void OnUiOpenEventHandler(UiTriggerEventArgs args);
    public void CloseUi()
    { 
        EmitSignal(SignalName.OnUiClose);
        IsUiOpen = false;
    }
    public void OpenUi(UserInterfaces userInterface, Component? UiComponent)
    {
        var uiArgs = new UiTriggerEventArgs()
        {
            UserInterface = userInterface,
            UiComponent = UiComponent,
        };

        EmitSignal(SignalName.OnUiOpen, uiArgs);

        IsUiOpen = true;
    }
}
