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
        Slots = new InventorySlot[SlotCount];

        for (int i = 0; i < InitSlots.Length; i++)
        {
            Slots[i] = InitSlots[i];
        }
    }

    public void CreateContainerContext()
    {   
        InventoryInterface.Instance.CreateContext(this);
    }

    public void OpenContainerInventory()
    {
        UiEventBus.Instance.OpenUi(UserInterfaces.ContainerInventory, this);
    }

}
