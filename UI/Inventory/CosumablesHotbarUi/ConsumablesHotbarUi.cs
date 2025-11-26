using DungeonLetter.Common;
using Godot;
using System;
using System.Linq;

public partial class ConsumablesHotbarUi : Control
{
    public ConsumableHotbarSlot[] hotbarSlots = null!;
    private int selectedIndex = -1;

    public override void _Ready()
    {
        InventoryInterface.Instance.OnEquipmentSlotItemChange += OnEquipmentItemChange;
        InventoryInterface.Instance.OnPlayerHandConfigChange += OnPlayerHandConfigChange;

        hotbarSlots = [.. GetNode<GridContainer>("GridContainer").GetChildren().OfType<ConsumableHotbarSlot>()];
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed(Inputs.CycleConsumables))
        {
            int endIndex = selectedIndex;
            if(selectedIndex == -1)
            {
                endIndex = hotbarSlots.Length - 1;
            }

            int i = NextIndex(selectedIndex);
            while (i != endIndex)
            {
                if (hotbarSlots[i].HasIcon())
                {
                    SelectSlot(i);
                    break;
                }
                i = NextIndex(i);
            }
        }
    }

    public void OnEquipmentItemChange(EquipmentItemChangeEventArgs args)
    {
        var index = GetConsumableSlotIndex(args.SlotType);
        if (index == -1)
        {
            return;
        }

        // Add or Remove Icon
        hotbarSlots[index].SetIcon(args.NewEquipment?.Icon);

        // If removing and it was selected, deselect it
        if (args.NewEquipment is null && selectedIndex == index)
        {
            hotbarSlots[index].Deselect();
            selectedIndex = -1;
        }
    }

    private void OnPlayerHandConfigChange(PlayerHandConfigChangeEventArgs args)
    {
        if(args.ActiveConfig != PlayerHandConfigs.Consumable)
        {
            foreach(var slot in hotbarSlots)
            {
                slot.Deselect();
                selectedIndex = -1;
            }
        }
    }

    private int GetConsumableSlotIndex(EquipmentSlotType slotType)
    {
        return slotType switch
        {
            EquipmentSlotType.Consumable1 => 0,
            EquipmentSlotType.Consumable2 => 1,
            EquipmentSlotType.Consumable3 => 2,
            EquipmentSlotType.Consumable4 => 3,
            EquipmentSlotType.Consumable5 => 4,
            _ => -1,
        };
    }

    private int NextIndex(int index)
    {
        if(index == hotbarSlots.Length -1 || index == -1)
        {
            return 0;
        }
        else
        {
            return index + 1;
        }
    }

    private void SelectSlot(int i)
    {
        // Deselect previous
        if (selectedIndex != -1)
        {
            hotbarSlots[selectedIndex].Deselect();
        }
        // Select new
        hotbarSlots[i].Select();
        selectedIndex = i;

        InventoryInterface.Instance.ConsumableSelected(i);
    }
}