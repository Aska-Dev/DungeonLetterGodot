using DungeonLetter.Common;
using Godot;
using System;

public partial class PlayerHandConfigChangeEventArgs : RefCounted
{
    public required PlayerHandConfigs ActiveConfig { get; set; }
}

public partial class EquipmentItemChangeEventArgs : RefCounted
{
    public EquipmentSlotType SlotType { get; set; }
    public required Equipment? NewEquipment { get; set; }
}

public partial class InventorySlotClickEventArgs : RefCounted
{
    public required int SlotIndex { get; set; }
    public required UserInterfaces ParentInterface { get; set; }

    public bool IsExclusiveItemSlot => ExclusiveItemId is not ExclusiveItemId.None;
    public ExclusiveItemId ExclusiveItemId { get; set; } = ExclusiveItemId.None;

    public bool IsEquipmentSlot => EquipmentSlotType is not EquipmentSlotType.NoEquipment;
    public EquipmentSlotType EquipmentSlotType { get; set; } = EquipmentSlotType.NoEquipment;
}

public partial class InventoryRefreshEventArgs : RefCounted
{
    public required InventorySlot[] Slots { get; set; }
    public required UserInterfaces ParentInterface { get; set; }
}

public partial class InventoryInterface : Node
{
    // Singleton Instance
    public static InventoryInterface Instance { get; private set; } = null!;

    // Public Properties
    public PlayerInventoryComponent PlayerInventory { get; set; } = null!;
    public PlayerEquipmentComponent PlayerEquipment { get; set; } = null!;
    public void IsOpen(bool value) => _isOpen = value;

    // Private Fields
    private InventoryContext? _context = null;
    private bool _isOpen = false;

    // Signals and Emitters
    /// INVENTORY
    [Signal] public delegate void OnInventorySlotClickedEventHandler(InventorySlotClickEventArgs args);
    public void InventorySlotClicked(InventorySlotClickEventArgs args) => EmitSignal(SignalName.OnInventorySlotClicked, args);

    /// PLAYER INVENTORY
    [Signal] public delegate void OnInventoryRefreshEventHandler(InventoryRefreshEventArgs args);
    public void RefreshInventory(InventoryRefreshEventArgs args) => EmitSignal(SignalName.OnInventoryRefresh, args);
    [Signal] public delegate void OnEquipmentSlotItemChangeEventHandler(EquipmentItemChangeEventArgs args);
    public void EquipmentSlotItemChanged(EquipmentItemChangeEventArgs args) => EmitSignal(SignalName.OnEquipmentSlotItemChange, args);
    [Signal] public delegate void OnPlayerHandConfigChangeEventHandler(PlayerHandConfigChangeEventArgs args);
    public void PlayerHandConfigChanged(PlayerHandConfigChangeEventArgs args) => EmitSignal(SignalName.OnPlayerHandConfigChange, args);


    public override void _Ready()
	{
		Instance = this;
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed(Inputs.ActionInventory))
        {
            if(_isOpen)
            {
                //GD.Print("Closing inventory.");
                _isOpen = false;
                if(_context is not null)
                {
                    _context.CloseContext();
                    _context = null;
                }
            }
            else
            {
                CreateContext();
            }
        }
    }

    public void CreateContext(ContainerInventoryComponent? containerInventory = null)
    {
        _isOpen = true;

        if (_context is not null)
        {
            //GD.Print("Closing existing inventory context.");
            _context.CloseContext();
        }

        _context = InventoryContext.Create(PlayerInventory, PlayerEquipment, containerInventory);
        GetTree().Root.AddChild(_context);
    }
}
