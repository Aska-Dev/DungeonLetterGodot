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

    // Slots: 0-8 = Inventory (3x3), 9-13 = Hotbar (5 slots)
    private Item?[] _slots;

    public override void _Ready()
    {
        base._Ready();
        _slots = new Item?[TotalSlots];
    }

    public Item? GetItemAt(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _slots.Length)
            return null;
        return _slots[slotIndex];
    }

    public bool SetItemAt(int slotIndex, Item? item)
    {
        if (slotIndex < 0 || slotIndex >= _slots.Length)
            return false;

        _slots[slotIndex] = item;
        EmitSignal(SignalName.InventoryChanged);
        return true;
    }

    public bool AddItem(Item item)
    {
        // Find first empty slot
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] == null)
            {
                _slots[i] = item;
                EmitSignal(SignalName.ItemAdded, item);
                EmitSignal(SignalName.InventoryChanged);
                return true;
            }
        }
        return false; // Inventory full
    }

    public bool MoveItem(int fromSlot, int toSlot)
    {
        if (fromSlot < 0 || fromSlot >= _slots.Length || toSlot < 0 || toSlot >= _slots.Length)
            return false;

        if (fromSlot == toSlot)
            return true;

        // Swap items
        var temp = _slots[toSlot];
        _slots[toSlot] = _slots[fromSlot];
        _slots[fromSlot] = temp;

        EmitSignal(SignalName.ItemMoved, fromSlot, toSlot);
        EmitSignal(SignalName.InventoryChanged);
        return true;
    }

    public bool RemoveItemAt(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _slots.Length)
            return false;

        _slots[slotIndex] = null;
        EmitSignal(SignalName.InventoryChanged);
        return true;
    }

    public bool IsSlotEmpty(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _slots.Length)
            return false;
        return _slots[slotIndex] == null;
    }

    public bool IsHotbarSlot(int slotIndex)
    {
        return slotIndex >= TotalInventorySlots && slotIndex < TotalSlots;
    }
}
