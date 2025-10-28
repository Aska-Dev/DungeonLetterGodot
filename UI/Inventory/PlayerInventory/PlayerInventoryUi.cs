using Godot;
using System;

public partial class PlayerInventoryUi : PanelContainer
{
	[ExportGroup("Dependencies")]
    [Export] public PackedScene InventorySlotUiScene { get; set; } = null!;
	[Export] public GridContainer ItemGrid { get; set; } = null!;

    public PlayerInventoryComponent? PlayerInventory { get; set; }

    public override void _Ready()
    {
        UiEventBus.Instance.OnUiOpen += OnOpenInventory;
        UiEventBus.Instance.OnUiClose += OnCloseInventory;
    }

    public void OnOpenInventory(UiTriggerEventArgs args)
    {
        if (args.UserInterface != UserInterfaces.PlayerInventory || args.UiComponent is not PlayerInventoryComponent playerInventory)
        {
            return;
        }

        PlayerInventory = playerInventory;
        PopulateGrid(playerInventory.Slots);
        Visible = true;
    }

    public void OnCloseInventory()
    {
        Visible = false;
        PlayerInventory = null;
    }

    private void PopulateGrid(InventorySlot[] slotDatas)
	{
        foreach (var child in ItemGrid.GetChildren())
        {
            child.QueueFree();
        }

        foreach(var slotData in slotDatas)
        {
            var slot = InventorySlotUiScene.Instantiate<InventorySlotUi>();

            if(!slotData.IsEmpty)
            {
                slot.SetItem(slotData.Item);
            }

            ItemGrid.AddChild(slot);
        }

    }
}
