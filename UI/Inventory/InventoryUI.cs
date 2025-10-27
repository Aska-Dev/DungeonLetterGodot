using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryUI : Control
{
    private InventoryComponent _inventory;
    private List<InventorySlot> _slots = new List<InventorySlot>();
    private GridContainer _inventoryGrid;
    private GridContainer _hotbarGrid;
    private int _draggedSlotIndex = -1;

    [Export] public NodePath InventoryComponentPath { get; set; }

    public override void _Ready()
    {
        // Get inventory component reference
        if (!string.IsNullOrEmpty(InventoryComponentPath))
        {
            _inventory = GetNode<InventoryComponent>(InventoryComponentPath);
        }

        if (_inventory == null)
        {
            GD.PrintErr("InventoryUI: InventoryComponent not found!");
            return;
        }

        SetupUI();
        _inventory.InventoryChanged += OnInventoryChanged;
        UpdateAllSlots();
    }

    private void SetupUI()
    {
        // Main container
        var mainVBox = new VBoxContainer();
        mainVBox.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(mainVBox);

        // Title
        var titleLabel = new Label();
        titleLabel.Text = "Inventar";
        titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
        titleLabel.AddThemeFontSizeOverride("font_size", 24);
        mainVBox.AddChild(titleLabel);

        mainVBox.AddChild(new HSeparator());

        // Inventory grid (3x3)
        var inventoryLabel = new Label();
        inventoryLabel.Text = "Inventar";
        inventoryLabel.HorizontalAlignment = HorizontalAlignment.Center;
        mainVBox.AddChild(inventoryLabel);

        _inventoryGrid = new GridContainer();
        _inventoryGrid.Columns = _inventory.InventoryColumns;
        _inventoryGrid.AddThemeConstantOverride("h_separation", 4);
        _inventoryGrid.AddThemeConstantOverride("v_separation", 4);
        mainVBox.AddChild(_inventoryGrid);

        // Create inventory slots (3x3 = 9 slots)
        for (int i = 0; i < _inventory.TotalInventorySlots; i++)
        {
            var slot = CreateSlot(i);
            _inventoryGrid.AddChild(slot);
            _slots.Add(slot);
        }

        mainVBox.AddChild(new HSeparator());

        // Hotbar (5 slots)
        var hotbarLabel = new Label();
        hotbarLabel.Text = "Hotbar";
        hotbarLabel.HorizontalAlignment = HorizontalAlignment.Center;
        mainVBox.AddChild(hotbarLabel);

        _hotbarGrid = new GridContainer();
        _hotbarGrid.Columns = _inventory.HotbarSize;
        _hotbarGrid.AddThemeConstantOverride("h_separation", 4);
        _hotbarGrid.AddThemeConstantOverride("v_separation", 4);
        mainVBox.AddChild(_hotbarGrid);

        // Create hotbar slots
        for (int i = _inventory.TotalInventorySlots; i < _inventory.TotalSlots; i++)
        {
            var slot = CreateSlot(i);
            _hotbarGrid.AddChild(slot);
            _slots.Add(slot);
        }

        // Background panel
        var bgStyleBox = new StyleBoxFlat();
        bgStyleBox.BgColor = new Color(0.1f, 0.1f, 0.1f, 0.95f);
        bgStyleBox.BorderColor = new Color(0.3f, 0.3f, 0.3f);
        bgStyleBox.SetBorderWidthAll(2);
        AddThemeStyleboxOverride("panel", bgStyleBox);

        // Center the inventory on screen
        SetAnchorsPreset(LayoutPreset.Center);
        CustomMinimumSize = new Vector2(400, 500);

        // Start hidden
        Visible = false;
    }

    private InventorySlot CreateSlot(int index)
    {
        var slot = new InventorySlot();
        slot.SlotIndex = index;
        slot.SlotClicked += OnSlotClicked;
        slot.SlotDragStarted += OnSlotDragStarted;
        slot.SlotDropped += OnSlotDropped;
        return slot;
    }

    private void OnSlotClicked(int slotIndex)
    {
        GD.Print($"Slot {slotIndex} clicked");
    }

    private void OnSlotDragStarted(int slotIndex)
    {
        _draggedSlotIndex = slotIndex;
        GD.Print($"Started dragging from slot {slotIndex}");
    }

    private void OnSlotDropped(int targetSlotIndex)
    {
        if (_draggedSlotIndex >= 0 && _draggedSlotIndex != targetSlotIndex)
        {
            GD.Print($"Dropped from slot {_draggedSlotIndex} to slot {targetSlotIndex}");
            _inventory.MoveItem(_draggedSlotIndex, targetSlotIndex);
        }
        _draggedSlotIndex = -1;
    }

    private void OnInventoryChanged()
    {
        UpdateAllSlots();
    }

    private void UpdateAllSlots()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].Item = _inventory.GetItemAt(i);
        }
    }

    public override void _Input(InputEvent @event)
    {
        // Toggle inventory with 'I' key or Tab
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (keyEvent.Keycode == Key.I || keyEvent.Keycode == Key.Tab)
            {
                ToggleInventory();
                GetViewport().SetInputAsHandled();
            }
        }
    }

    public void ToggleInventory()
    {
        Visible = !Visible;
        
        // Pause/unpause game when inventory is open
        if (Visible)
        {
            GetTree().Paused = true;
        }
        else
        {
            GetTree().Paused = false;
        }
    }

    public void OpenInventory()
    {
        Visible = true;
        GetTree().Paused = true;
    }

    public void CloseInventory()
    {
        Visible = false;
        GetTree().Paused = false;
    }
}
