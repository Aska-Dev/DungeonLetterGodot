using Godot;
using System;

public partial class ContainerInventoryUi : PanelContainer
{
    [ExportGroup("Dependencies")]
    [Export] public PackedScene InventorySlotUiScene { get; set; } = null!;
    [Export] public GridContainer ItemGrid { get; set; } = null!;

    public ContainerInventoryComponent? ContainerInventory { get; set; }

    public override void _Ready()
    {
        UiEventBus.Instance.OnUiOpen += OnOpenInventory;
        UiEventBus.Instance.OnUiClose += OnCloseInventory;
    }

    public void OnOpenInventory(UiTriggerEventArgs args)
    {
        if(args.UserInterface != UserInterfaces.ContainerInventory || args.UiComponent is not ContainerInventoryComponent containerInventory)
        {
            return;
        }

        Visible = true;

        ContainerInventory = containerInventory;
        PopulateGrid(containerInventory.Slots);
    }

    public void OnCloseInventory()
    { 
        Visible = false;
        ContainerInventory = null;
    }

    private void PopulateGrid(InventorySlot[] slotDatas)
    {
        foreach (var child in ItemGrid.GetChildren())
        {
            child.QueueFree();
        }

        foreach (var slotData in slotDatas)
        {
            var slot = InventorySlotUiScene.Instantiate<InventorySlotUi>();

            if (slotData is not null && !slotData.IsEmpty)
            {
                slot.SetItem(slotData.Item!);
            }

            ItemGrid.AddChild(slot);
        }

    }
}
