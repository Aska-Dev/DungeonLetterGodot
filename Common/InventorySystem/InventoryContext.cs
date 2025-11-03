using DungeonLetter.Common;
using Godot;
using System;

public partial class InventoryContext : Node
{
    private PackedScene _grabbedSlotScene = GD.Load<PackedScene>("res://UI/Inventory/GrabbedSlotDisplay/GrabbedSlotDisplayUi.tscn");

    public PlayerInventoryComponent InventoryComponent { get; set; } = null!;
    public PlayerEquipmentComponent EquipmentComponent { get; set; } = null!;
    public ContainerInventoryComponent? ContainerComponent { get; set; }

    public GrabbedSlotDisplayUi GrabbedSlotDisplay { get; set; } = null!;

    public static InventoryContext Create(PlayerInventoryComponent playerInventory, PlayerEquipmentComponent equipmentComponent, ContainerInventoryComponent? containerInventory = null)
    {
        //GD.Print("Creating InventoryContext.");

        var context = new InventoryContext();
        context.InventoryComponent = playerInventory;
        context.EquipmentComponent = equipmentComponent;
        context.ContainerComponent = containerInventory;

        return context; 
    }

    public override void _Ready()
    {
        //GD.Print("InventoryContext: _Ready called.");

        // Initialize Grabbed Slot Display UI
        GrabbedSlotDisplay = _grabbedSlotScene.Instantiate<GrabbedSlotDisplayUi>();

        // Add to UI Overlay
        var overlay = GetTree().GetFirstNodeInGroup("ui.overlay") as CanvasLayer;
        if(overlay is null)
        {
            GD.PushError("InventoryContext: Unable to find UI Overlay CanvasLayer in scene tree.");
        }
        overlay.AddChild(GrabbedSlotDisplay);

        // Open Inventories
        OpenAllInventories();

        // Setup signals
        InventoryInterface.Instance.OnInventorySlotClicked += OnSlotClick;
    }

    public override void _ExitTree()
    {
        InventoryInterface.Instance.OnInventorySlotClicked -= OnSlotClick;
        //GrabbedSlotDisplay.QueueFree();
    }

    public void OnSlotClick(InventorySlotClickEventArgs args)
    {
        var clickedSlot = GetClickedSlot(args);

        // If no item is currently grabbed
        if (GrabbedSlotDisplay.Slot is null)
        {
            // Pick up item
            if(!clickedSlot.IsEmpty)
            {
                GrabbedSlotDisplay.UpdateDisplay(clickedSlot);
                SetClickedSlot(args, new InventorySlot());
            }
        }
        else
        {
            if(args.IsExclusiveItemSlot && GrabbedSlotDisplay.Slot.Item?.ExclusiveItemId != args.ExclusiveItemId)
            {
                // Invalid slot for this item type
                return;
            }

            // Place item
            SetClickedSlot(args, GrabbedSlotDisplay.Slot);

            // If there is no item on the clicked slot, clear display
            if (clickedSlot.IsEmpty)
            {
                GrabbedSlotDisplay.ClearDisplay();
            }
            // Else, update display to the clicked slot item
            else
            {
                GrabbedSlotDisplay.UpdateDisplay(clickedSlot);
            }
        }

        RefreshInventory(args.ParentInterface);
    }

    private void RefreshInventory(UserInterfaces parentInterface)
    {
        InventorySlot[] slots = [];
        if (parentInterface == UserInterfaces.PlayerInventory)
        {
            slots = InventoryComponent.Slots;
        }
        else if(parentInterface == UserInterfaces.PlayerEquipment)
        {
            slots = EquipmentComponent.Slots;
        }
        else if (parentInterface == UserInterfaces.ContainerInventory && ContainerComponent is not null)
        {
            slots = ContainerComponent!.Slots;
        }

        var refreshArgs = new InventoryRefreshEventArgs()
        {
            ParentInterface = parentInterface,
            Slots = slots
        };
        InventoryInterface.Instance.RefreshInventory(refreshArgs);
    }

    private InventorySlot GetClickedSlot(InventorySlotClickEventArgs args)
    {
        var slot = new InventorySlot();

        if (args.ParentInterface == UserInterfaces.PlayerInventory)
        {
            slot = InventoryComponent.Slots[args.SlotIndex];
        }
        else if (args.ParentInterface == UserInterfaces.PlayerEquipment)
        {
            slot = EquipmentComponent.Slots[args.SlotIndex];
        }
        else if (args.ParentInterface == UserInterfaces.ContainerInventory && ContainerComponent is not null)
        {
            slot = ContainerComponent!.Slots[args.SlotIndex];
        }

        if(slot is not null)
        {
            return slot;
        }

        return new InventorySlot();
    }

    private void SetClickedSlot(InventorySlotClickEventArgs args, InventorySlot newSlot)
    {
        if (args.ParentInterface == UserInterfaces.PlayerInventory)
        {
            InventoryComponent.Slots[args.SlotIndex] = newSlot;
        }
        else if(args.ParentInterface == UserInterfaces.PlayerEquipment)
        {
            if(args.EquipmentSlotType != EquipmentSlotType.NoEquipment)
            {
                EquipmentComponent.Slots[args.SlotIndex] = newSlot;

                var newEquipment = newSlot.Item as Equipment;
                InventoryInterface.Instance.EquipmentSlotItemChanged(new EquipmentItemChangeEventArgs()
                {
                    SlotType = args.EquipmentSlotType,
                    NewEquipment = newEquipment
                });
            }
        }
        else if (args.ParentInterface == UserInterfaces.ContainerInventory && ContainerComponent is not null)
        {
            ContainerComponent!.Slots[args.SlotIndex] = newSlot;
        }
    }

    public void OpenAllInventories()
    {
        InventoryComponent.OnOpenInventory();
        ContainerComponent?.OpenContainerInventory();
    }

    public void CloseContext()
    {
        UiEventBus.Instance.CloseUi();
        QueueFree();
    }

}
