using Godot;
using System;

public partial class InventorySlot : PanelContainer
{
    [Signal] public delegate void SlotClickedEventHandler(int slotIndex);
    [Signal] public delegate void SlotDragStartedEventHandler(int slotIndex);
    [Signal] public delegate void SlotDroppedEventHandler(int slotIndex);

    private int _slotIndex;
    private Item? _item;
    private TextureRect _itemIcon;
    private Label _itemNameLabel;
    private bool _isDragging;
    private Control _dragPreview;

    public int SlotIndex
    {
        get => _slotIndex;
        set => _slotIndex = value;
    }

    public Item? Item
    {
        get => _item;
        set
        {
            _item = value;
            UpdateDisplay();
        }
    }

    public override void _Ready()
    {
        // Create UI structure
        var vBox = new VBoxContainer();
        vBox.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(vBox);

        _itemIcon = new TextureRect();
        _itemIcon.CustomMinimumSize = new Vector2(64, 64);
        _itemIcon.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
        _itemIcon.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
        vBox.AddChild(_itemIcon);

        _itemNameLabel = new Label();
        _itemNameLabel.HorizontalAlignment = HorizontalAlignment.Center;
        _itemNameLabel.AddThemeColorOverride("font_color", new Color(1, 1, 1, 0.8f));
        vBox.AddChild(_itemNameLabel);

        // Setup visual style
        var styleBox = new StyleBoxFlat();
        styleBox.BgColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        styleBox.BorderColor = new Color(0.5f, 0.5f, 0.5f);
        styleBox.SetBorderWidthAll(2);
        AddThemeStyleboxOverride("panel", styleBox);

        CustomMinimumSize = new Vector2(80, 80);

        // Enable mouse input
        MouseFilter = MouseFilterEnum.Stop;
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Left)
            {
                if (mouseButton.Pressed)
                {
                    if (_item != null)
                    {
                        _isDragging = true;
                        EmitSignal(SignalName.SlotDragStarted, _slotIndex);
                    }
                    EmitSignal(SignalName.SlotClicked, _slotIndex);
                }
                else if (_isDragging)
                {
                    _isDragging = false;
                }
            }
        }
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return data.VariantType == Variant.Type.Int;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        if (data.VariantType == Variant.Type.Int)
        {
            EmitSignal(SignalName.SlotDropped, _slotIndex);
        }
    }

    public override Variant _GetDragData(Vector2 atPosition)
    {
        if (_item == null)
            return default;

        // Create drag preview
        var preview = new TextureRect();
        preview.Texture = _itemIcon.Texture;
        preview.CustomMinimumSize = new Vector2(64, 64);
        preview.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
        preview.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
        preview.Modulate = new Color(1, 1, 1, 0.7f);
        SetDragPreview(preview);

        return _slotIndex;
    }

    private void UpdateDisplay()
    {
        if (_item != null)
        {
            _itemNameLabel.Text = _item.Name;
            // Try to load the item icon from the model
            if (_item.Model != null && !string.IsNullOrEmpty(_item.Model.Path))
            {
                var texture = GD.Load<Texture2D>(_item.Model.Path);
                if (texture != null)
                {
                    _itemIcon.Texture = texture;
                }
            }
            else
            {
                _itemIcon.Texture = null;
            }
        }
        else
        {
            _itemNameLabel.Text = "";
            _itemIcon.Texture = null;
        }
    }

    public void Highlight(bool enabled)
    {
        var styleBox = GetThemeStylebox("panel") as StyleBoxFlat;
        if (styleBox != null)
        {
            styleBox.BorderColor = enabled ? new Color(1, 1, 0) : new Color(0.5f, 0.5f, 0.5f);
        }
    }
}
