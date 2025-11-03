using Godot;
using System;

public partial class PlayerInventoryUi : Control
{
    [ExportGroup("Dependencies")]
    [Export] public PackedScene InventorySlotUiScene { get; set; } = null!;
    [Export] public GridContainer ItemGrid { get; set; } = null!;
    [Export] public Control EquipmentContainer { get; set; } = null!;

    private InventorySlot[]? _pendingSlots = null;
    private bool _rebuild = false;

    public override void _Ready()
    {
        UiEventBus.Instance.OnUiOpen += OnOpenInventory;
        UiEventBus.Instance.OnUiClose += OnCloseInventory;
        InventoryInterface.Instance.OnInventoryRefresh += OnUpdateInventory;
    }

    public override void _ExitTree()
    {
        UiEventBus.Instance.OnUiOpen -= OnOpenInventory;
        UiEventBus.Instance.OnUiClose -= OnCloseInventory;
        InventoryInterface.Instance.OnInventoryRefresh -= OnUpdateInventory;
    }

    public override void _Process(double delta)
    {
        if (_rebuild && _pendingSlots != null)
        {
            _rebuild = false;
            PopulateGrid(_pendingSlots);
        }
    }

    public void OnOpenInventory(UiTriggerEventArgs args)
    {
        if (args.UserInterface != UserInterfaces.PlayerInventory || args.UiComponent is not PlayerInventoryComponent playerInventory)
        {
            return;
        }

        PopulateGrid(playerInventory.Slots);
        Visible = true;
    }

    public void OnCloseInventory()
    {

        Visible = false;
    }

    public void OnUpdateInventory(InventoryRefreshEventArgs args)
    {
        if (args.Slots is null)
        {
            return;
        }

        if(args.ParentInterface == UserInterfaces.PlayerInventory)
        {
            UpdateInventoryDisplay(args.Slots);
        }

        if(args.ParentInterface == UserInterfaces.PlayerEquipment)
        {
            UpdateEquipmentDisplay(args.Slots);
        }
    }

    private void RequestRebuild(InventorySlot[] slotDatas)
    {
        _pendingSlots = slotDatas;
        _rebuild = true;
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
            slot.ParentInterface = UserInterfaces.PlayerInventory;

            if (!slotData.IsEmpty)
            {
                slot.SetItem(slotData.Item!);
            }

            ItemGrid.AddChild(slot);
        }
    }

    private void UpdateEquipmentDisplay(InventorySlot[] slotDatas)
    {
        var equipmentSlots = EquipmentContainer.GetChildren();

        for (var i = 0; i < slotDatas.Length; i++)
        {
            var slotData = slotDatas[i];
            var slot = equipmentSlots[i] as InventorySlotUi;

            if (!slotData.IsEmpty)
            {
                slot!.SetItem(slotData.Item!);
            }
            else
            {
                slot.Reset();
            }
        }
    }

    private void UpdateInventoryDisplay(InventorySlot[] slotDatas)
    {
        var itemSlots = ItemGrid.GetChildren();
        for (var i = 0; i < slotDatas.Length; i++)
        {
            var slotData = slotDatas[i];
            var slot = itemSlots[i] as InventorySlotUi;

            if (!slotData.IsEmpty)
            {
                slot!.SetItem(slotData.Item!);
            }
            else
            {
                slot.Reset();
            }
        }
    }
}
