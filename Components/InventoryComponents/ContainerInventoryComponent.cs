using Godot;
using System;

[GlobalClass]
public partial class ContainerInventoryComponent : Component
{
    [ExportGroup("Config")]
    [Export] public int SlotCount { get; set; } = 12;

    [ExportGroup("Content")]
    [Export] public InventorySlot[] InitSlots { get; set; } = [];

    public InventorySlot[] Slots { get; private set; } = [];

    public override void _Ready()
    {
        base._Ready();

        Slots = new InventorySlot[SlotCount];

        for (int i = 0; i < InitSlots.Length; i++)
        {
            Slots[i] = InitSlots[i];
        }

    }

    public void OnOpenContainer()
    {
        UiEventBus.Instance.OpenUi(UserInterfaces.ContainerInventory, this);
        OpenPlayerInventory();
    }

    private void OpenPlayerInventory()
    {
        var playerInventory = GetTree().GetFirstNodeInGroup("player.inventory") as PlayerInventoryComponent;
        if (playerInventory is not null)
        {
            playerInventory.OnOpenInventory();
        }
    }
}
