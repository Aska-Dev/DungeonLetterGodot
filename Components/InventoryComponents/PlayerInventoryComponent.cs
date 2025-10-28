using Godot;
using System;
using System.Collections.Generic;
using System.Linq;



[GlobalClass]
public partial class PlayerInventoryComponent : Component
{
    [ExportGroup("Config")]
    [Export] public int SlotCount { get; set; } = 12;
    [Export] public int HotbarSize { get; set; } = 4;

    [ExportGroup("Content")]
    [Export] public InventorySlot[] InitSlots { get; set; } = [];

    public InventorySlot[] Slots { get; private set; } = [];

    private bool _isOpen = false;

    public override void _Ready()
    {
        AddToGroup("player.inventory");

        Slots = new InventorySlot[SlotCount];
        Array.Fill(Slots, new InventorySlot());

        for (int i = 0; i < InitSlots.Length; i++)
        {
            Slots[i] = InitSlots[i];
        }

        UiEventBus.Instance.OnUiClose += CloseInventory;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("player_inventory_toggle"))
        {
            if (_isOpen)
            {
                UiEventBus.Instance.CloseUi();
            }
            else
            {
                OnOpenInventory();
            }
        }
    }

    public void AddItem(Item item)
    {
        var emptySlot = Slots.FirstOrDefault(slot => slot.IsEmpty);
        if (emptySlot != null)
        {
            emptySlot.Item = item;
        }
    }

    public void OnOpenInventory()
    {
        _isOpen = true;

        ProcessMode = ProcessModeEnum.Always;
        Input.MouseMode = Input.MouseModeEnum.Visible;

        var player = GetParent<Player>();
        player.ProcessMode = ProcessModeEnum.Disabled;

        UiEventBus.Instance.OpenUi(UserInterfaces.PlayerInventory, this);
    }

    public void CloseInventory()
    {
        _isOpen = false;

        var player = GetParent<Player>();
        player.ProcessMode = ProcessModeEnum.Inherit;
        
        ProcessMode = ProcessModeEnum.Inherit;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
}


