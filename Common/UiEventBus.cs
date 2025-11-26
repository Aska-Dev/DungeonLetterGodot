using Godot;
using System;

public enum UserInterfaces
{
    PlayerInventory,
    PlayerEquipment,
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

    /// HEALTH BAR
    [Signal]
    public delegate void OnHealthBarUpdateEventHandler(float currentHealth, float maxHealth);
    public void UpdateHealthBar(float currentHealth, float maxHealth) => EmitSignal(SignalName.OnHealthBarUpdate, currentHealth, maxHealth);

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

    /// TOOLTIP
    [Signal] public delegate void OnItemTooltipToggleEventHandler(bool on, Item? item);
    public void ToggleItemTooltip(bool on, Item? item = null) => EmitSignal(SignalName.OnItemTooltipToggle, on, item);
}
