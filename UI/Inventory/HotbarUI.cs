using Godot;
using System;

/// <summary>
/// Simple hotbar UI that displays the 5 hotbar slots from the inventory
/// Can be displayed at the bottom of the screen during gameplay
/// </summary>
public partial class HotbarUI : Control
{
    private InventoryComponent _inventory;
    private HBoxContainer _hotbarContainer;
    private InventorySlot[] _hotbarSlots;
    private int _selectedSlotIndex = 0;

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
            GD.PrintErr("HotbarUI: InventoryComponent not found!");
            return;
        }

        SetupUI();
        _inventory.InventoryChanged += OnInventoryChanged;
        UpdateHotbarSlots();
    }

    private void SetupUI()
    {
        _hotbarContainer = new HBoxContainer();
        _hotbarContainer.AddThemeConstantOverride("separation", 4);
        AddChild(_hotbarContainer);

        _hotbarSlots = new InventorySlot[_inventory.HotbarSize];

        // Create hotbar slots
        for (int i = 0; i < _inventory.HotbarSize; i++)
        {
            var slot = new InventorySlot();
            slot.SlotIndex = _inventory.TotalInventorySlots + i; // Hotbar starts after inventory
            _hotbarContainer.AddChild(slot);
            _hotbarSlots[i] = slot;

            // Add number label
            var numberLabel = new Label();
            numberLabel.Text = (i + 1).ToString();
            numberLabel.Position = new Vector2(5, 5);
            slot.AddChild(numberLabel);
        }

        // Position at bottom center of screen
        SetAnchorsPreset(LayoutPreset.BottomWide);
        AnchorTop = 1.0f;
        AnchorBottom = 1.0f;
        OffsetTop = -100;
        OffsetBottom = -10;
        GrowHorizontal = GrowDirection.Both;

        _hotbarContainer.SetAnchorsPreset(LayoutPreset.Center);

        // Highlight first slot
        UpdateSelectedSlot();
    }

    private void OnInventoryChanged()
    {
        UpdateHotbarSlots();
    }

    private void UpdateHotbarSlots()
    {
        for (int i = 0; i < _hotbarSlots.Length; i++)
        {
            int slotIndex = _inventory.TotalInventorySlots + i;
            _hotbarSlots[i].Item = _inventory.GetItemAt(slotIndex);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            // Select hotbar slot with number keys 1-5
            if (keyEvent.Keycode >= Key.Key1 && keyEvent.Keycode <= Key.Key5)
            {
                int slotIndex = (int)keyEvent.Keycode - (int)Key.Key1;
                if (slotIndex < _inventory.HotbarSize)
                {
                    SelectSlot(slotIndex);
                }
            }
            // Mouse wheel to cycle through slots
            else if (keyEvent.Keycode == Key.Quoteleft) // Mouse wheel up alternative
            {
                SelectSlot((_selectedSlotIndex - 1 + _inventory.HotbarSize) % _inventory.HotbarSize);
            }
            else if (keyEvent.Keycode == Key.Equal) // Mouse wheel down alternative
            {
                SelectSlot((_selectedSlotIndex + 1) % _inventory.HotbarSize);
            }
        }
        else if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed)
            {
                if (mouseButton.ButtonIndex == MouseButton.WheelUp)
                {
                    SelectSlot((_selectedSlotIndex - 1 + _inventory.HotbarSize) % _inventory.HotbarSize);
                    GetViewport().SetInputAsHandled();
                }
                else if (mouseButton.ButtonIndex == MouseButton.WheelDown)
                {
                    SelectSlot((_selectedSlotIndex + 1) % _inventory.HotbarSize);
                    GetViewport().SetInputAsHandled();
                }
            }
        }
    }

    private void SelectSlot(int index)
    {
        if (index < 0 || index >= _inventory.HotbarSize)
            return;

        _selectedSlotIndex = index;
        UpdateSelectedSlot();
    }

    private void UpdateSelectedSlot()
    {
        for (int i = 0; i < _hotbarSlots.Length; i++)
        {
            _hotbarSlots[i].Highlight(i == _selectedSlotIndex);
        }
    }

    public Item? GetSelectedItem()
    {
        int slotIndex = _inventory.TotalInventorySlots + _selectedSlotIndex;
        return _inventory.GetItemAt(slotIndex);
    }

    public int GetSelectedSlotIndex()
    {
        return _selectedSlotIndex;
    }
}
