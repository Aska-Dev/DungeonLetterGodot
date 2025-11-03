using Godot;
using System;

public partial class HotbarUi : Control
{
	[ExportCategory("Dependencies")]
	[Export] public required PanelContainer PrimaryHand { get; set; }
    [Export] public required TextureRect MainHand1 { get; set; }
	[Export] public required TextureRect OffHand1 { get; set; }
    [Export] public required PanelContainer SecondaryHand { get; set; }
    [Export] public required TextureRect MainHand2 { get; set; }
	[Export] public required TextureRect OffHand2 { get; set; }

	public override void _Ready()
	{
		InventoryInterface.Instance.OnEquipmentSlotItemChange += OnEquipmentItemChange;
		InventoryInterface.Instance.OnPlayerHandConfigChange += OnPlayerHandConfigChange;
    }

	public void OnPlayerHandConfigChange(PlayerHandConfigChangeEventArgs args)
	{
		if(args.ActiveConfig == PlayerHandConfigs.Primary)
		{
			SetHandInactive(SecondaryHand);
			SetHandActive(PrimaryHand);
        }
		else if(args.ActiveConfig == PlayerHandConfigs.Secondary)
		{
			SetHandInactive(PrimaryHand);
			SetHandActive(SecondaryHand);
        }
    }

    public void OnEquipmentItemChange(EquipmentItemChangeEventArgs args)
	{
		if(args.SlotType == EquipmentSlotType.MainHand1)
		{
			if(args.NewEquipment is not null)
			{
				MainHand1.Texture = args.NewEquipment.Icon;
			}
			else
			{
				MainHand1.Texture = null;
			}
		}
		else if(args.SlotType == EquipmentSlotType.OffHand1)
		{
			if(args.NewEquipment is not null)
			{
				OffHand1.Texture = args.NewEquipment.Icon;
			}
			else
			{
				OffHand1.Texture = null;
			}
        }
		else if(args.SlotType == EquipmentSlotType.MainHand2)
        {
			if(args.NewEquipment is not null)
            {
				MainHand2.Texture = args.NewEquipment.Icon;
            }
			else
            {
				MainHand2.Texture = null;
            }
        }
		else if(args.SlotType == EquipmentSlotType.OffHand2)
        {
			if(args.NewEquipment is not null)
            {
				OffHand2.Texture = args.NewEquipment.Icon;
            }
			else
            {
				OffHand2.Texture = null;
            }
        }
    }

	private void SetHandActive(PanelContainer hand)
	{
		var color = hand.Modulate;
		color.A = 1f;
		hand.Modulate = color;
    }

	private void SetHandInactive(PanelContainer hand)
	{
		var color = hand.Modulate;
		color.A = 0.5f;
		hand.Modulate = color;
    }
}
