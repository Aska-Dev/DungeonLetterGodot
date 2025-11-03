using Godot;
using System;

[GlobalClass]
public partial class PlayerEquipmentComponent : Component
{
    [ExportCategory("Dependencies")]
    [Export] public required StatsComponent Stats { get; set; }
    [Export] public required PlayerHands PlayerHands { get; set; }

    [ExportCategory("Content")]
    [Export] public InventorySlot[] Slots { get; set; } = [];

    // EQUIPMENT SLOTS
    // -------------------------------------

    // Armor Slots
    public Armor? Head { get; set; }
    public Armor? Body { get; set; }
    public Armor? Legs { get; set; }
    public Armor? Boots { get; set; }

    // Weapon Slots
    public Holdable? MainHand1 { get; set; }
    public Holdable? MainHand2 { get; set; }
    public Holdable? OffHand1 { get; set; }
    public Holdable? OffHand2 { get; set; }

    public override void _Ready()
    {
        // Register this component with the InventoryInterface singleton
        InventoryInterface.Instance.PlayerEquipment = this;

        // Subscribe to equipment change events
        InventoryInterface.Instance.OnEquipmentSlotItemChange += OnEquipmentChanged;
    }

    public void OnEquipmentChanged(EquipmentItemChangeEventArgs args)
    {
        var oldEquipment = GetBySlot(args.SlotType);
        if(oldEquipment is not null)
        {
            oldEquipment.RemoveModifiers(Stats);
        }

        SetBySlot(args.SlotType, args.NewEquipment);
        if(args.NewEquipment is not null)
        {
            args.NewEquipment.ApplyModifiers(Stats);
        }

        HandleHandEquipmentChange(args);
    }

    private void HandleHandEquipmentChange(EquipmentItemChangeEventArgs args)
    {
        if(args.NewEquipment is not Holdable holdableItem)
        {
            return;
        }

        // Update PlayerHands configurations based on equipment changes
        switch (args.SlotType)
        {
            case EquipmentSlotType.MainHand1:
                PlayerHands.PrimaryConfig.MainHandItem = holdableItem;
                break;
            case EquipmentSlotType.OffHand1:
                PlayerHands.PrimaryConfig.OffHandItem = holdableItem;
                break;
            case EquipmentSlotType.MainHand2:
                PlayerHands.SecondaryConfig.MainHandItem = holdableItem;
                break;
            case EquipmentSlotType.OffHand2:
                PlayerHands.SecondaryConfig.OffHandItem = holdableItem;
                break;
            default:
                return;
        }

        if(args.SlotType is EquipmentSlotType.MainHand1 or EquipmentSlotType.OffHand1 && PlayerHands.ActiveConfig is PlayerHandConfigs.Primary)
        {
            PlayerHands.LoadConfig(PlayerHands.PrimaryConfig, PlayerHands.ActiveConfig);
        }
        else if(args.SlotType is EquipmentSlotType.MainHand2 or EquipmentSlotType.OffHand2 && PlayerHands.ActiveConfig is PlayerHandConfigs.Secondary)
        {
            PlayerHands.LoadConfig(PlayerHands.SecondaryConfig, PlayerHands.ActiveConfig);
        }
    }

    private Equipment? GetBySlot(EquipmentSlotType type) => type switch
    {
        EquipmentSlotType.Head => Head,
        EquipmentSlotType.Body => Body,
        EquipmentSlotType.Legs => Legs,
        EquipmentSlotType.Boots => Boots,
        EquipmentSlotType.MainHand1 => MainHand1,
        EquipmentSlotType.MainHand2 => MainHand2,
        EquipmentSlotType.OffHand1 => OffHand1,
        EquipmentSlotType.OffHand2 => OffHand2,
        _ => null
    };

    private void SetBySlot(EquipmentSlotType type, Equipment? item)
    {
        if(!IsCompatible(type, item))
        {
            return;
        }
        
        switch (type)
        {
            case EquipmentSlotType.Head:
                Head = item as Armor;
                break;
            case EquipmentSlotType.Body:
                Body = item as Armor;
                break;
            case EquipmentSlotType.Legs:
                Legs = item as Armor;
                break;
            case EquipmentSlotType.Boots:
                Boots = item as Armor;
                break;
            case EquipmentSlotType.MainHand1:
                MainHand1 = item as Weapon;
                break;
            case EquipmentSlotType.MainHand2:
                MainHand2 = item as Weapon;
                break;
            case EquipmentSlotType.OffHand1:
                OffHand1 = item as Weapon;
                break;
            case EquipmentSlotType.OffHand2:
                OffHand2 = item as Weapon;
                break;
        }
    }

    private bool IsCompatible(EquipmentSlotType slot, Equipment? item)
    {
        if (item is null) return true;

        return slot switch
        {
            EquipmentSlotType.Head => item.Type == EquipmentTypes.Headgear,
            EquipmentSlotType.Body => item.Type == EquipmentTypes.BodyArmor,
            EquipmentSlotType.Legs => item.Type == EquipmentTypes.LegArmor,
            EquipmentSlotType.Boots => item.Type == EquipmentTypes.Boots,
            EquipmentSlotType.MainHand1 or
            EquipmentSlotType.MainHand2 => item.Type == EquipmentTypes.MainHand,
            EquipmentSlotType.OffHand1 or
            EquipmentSlotType.OffHand2 => item.Type == EquipmentTypes.OffHand,
            _ => false
        };
    }
}
