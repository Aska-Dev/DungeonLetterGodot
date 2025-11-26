using Godot;
using System;
using System.Collections.Generic;
using System.Linq;



[GlobalClass]
public partial class PlayerInventoryComponent : Component
{
    //[ExportGroup("Config")]
    //[Export] public int HotbarSize { get; set; } = 4;

    [ExportGroup("Content")]
    [Export] public InventorySlot[] Slots { get; set; } = [];

    public override void _Ready()
    {
        InventoryInterface.Instance.PlayerInventory = this;

        UiEventBus.Instance.OnUiClose += CloseInventory;
    }

    public void OnOpenInventory()
    {
        var player = GetParent<Player>();
        var rayComponent = player.Components.Get<InteractionRayComponent>();
        rayComponent.DisableInteraction();

        UiEventBus.Instance.OpenUi(UserInterfaces.PlayerInventory, this);

        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public void CloseInventory()
    {
        var player = GetParent<Player>();
        var rayComponent = player.Components.Get<InteractionRayComponent>();
        rayComponent.EnableInteraction();

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
}