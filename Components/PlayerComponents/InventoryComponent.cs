using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryComponent : Component
{
    [Signal] public delegate void ItemAddedEventHandler(Item item);
    [Signal] public delegate void ItemMovedEventHandler(int fromSlot, int toSlot);
    [Signal] public delegate void InventoryChangedEventHandler();

    [ExportGroup("Settings")]
    [Export] public int InventoryRows { get; set; } = 3;
    [Export] public int InventoryColumns { get; set; } = 3;
    [Export] public int HotbarSize { get; set; } = 5;

    public int TotalInventorySlots => InventoryRows * InventoryColumns;
    public int TotalSlots => TotalInventorySlots + HotbarSize;

    private Item?[] _slots = null!;

    public override void _Ready()
    {
        _slots = new Item?[TotalSlots];
    }

    public Item? GetItemAt(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _slots.Length)
        {
            return null;
        }

        return _slots[slotIndex];
    }

    public void SetItemAt(int slotIndex, Item? item)
    {
        if (slotIndex < 0 || slotIndex >= _slots.Length)
        {
            GD.PrintErr("Invalid slot index for setting item.");
            return;
        }

        _slots[slotIndex] = item;
        EmitSignal(SignalName.InventoryChanged);
    }

    public void AddItem(Item item)
    {
        // Find first empty slot
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] == null)
            {
                _slots[i] = item;
                EmitSignal(SignalName.ItemAdded, item);
                EmitSignal(SignalName.InventoryChanged);
            }
        }
    }

    public void MoveItem(int fromSlot, int toSlot)
    {
        if (fromSlot < 0 || fromSlot >= _slots.Length || toSlot < 0 || toSlot >= _slots.Length)
        {
            GD.PrintErr("Invalid slot indices for moving item.");
            return;
        }

        // Swap items
        var temp = _slots[toSlot];
        _slots[toSlot] = _slots[fromSlot];
        _slots[fromSlot] = temp;

        EmitSignal(SignalName.ItemMoved, fromSlot, toSlot);
        EmitSignal(SignalName.InventoryChanged);
    }

    public void RemoveItemAt(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _slots.Length)
        {
            GD.PrintErr("Invalid slot index for removing item.");
            return;
        }

        _slots[slotIndex] = null;
        EmitSignal(SignalName.InventoryChanged);
    }

    public bool IsSlotEmpty(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _slots.Length)
        {
            GD.PrintErr("Invalid slot index for checking emptiness.");
            return false;
        }

        return _slots[slotIndex] == null;
    }

    public bool IsHotbarSlot(int slotIndex)
    {
        return slotIndex >= TotalInventorySlots && slotIndex < TotalSlots;
    }
}
