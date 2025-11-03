using Godot;
using System;

public enum EquipmentSlotType
{
	None,
	Head,
	Body,
	Legs,
	Boots,
	MainHand1,
	MainHand2,
	OffHand1,
	OffHand2
}

public partial class InventorySlotUi : PanelContainer
{
    [Export] public required TextureRect IconRenderer { get; set; }
	[Export] public CompressedTexture2D? BackgroundTexture { get; set; } = null;
    [Export] public ItemType ExclusiveType { get; set; } = ItemType.Item;
	[Export] public EquipmentSlotType EquipmentSlotType { get; set; } = EquipmentSlotType.None;
    [Export]public UserInterfaces ParentInterface { get; set; }

	private Item? _item = null;

	public override void _Ready()
	{
		MouseEntered += ShowTooltip;
        MouseExited += () => UiEventBus.Instance.ToggleItemTooltip(false);

		SetExlusiveTypeTooltip();

		if(_item is null)
		{
			IconRenderer.Texture = BackgroundTexture;
		}
    }

	public void ShowTooltip()
	{
		if(_item is not null)
		{
            UiEventBus.Instance.ToggleItemTooltip(true, _item);
        }
		else
		{
            UiEventBus.Instance.ToggleItemTooltip(false);
        }
	}

    public void SetItem(Item item)
	{
		ClearExclusiveTypeTooltip();

        if (EquipmentSlotType != EquipmentSlotType.None)
		{
			var equipChangeArgs = new EquipmentItemChangeEventArgs()
			{
				SlotType = EquipmentSlotType,
				NewItem = item
			};
			InventoryInterface.Instance.EquipmentSlotItemChanged(equipChangeArgs);
        }

		_item = item;
        IconRenderer.Texture = item.Icon;
    }

	public void Reset()
	{
		_item = null;
		IconRenderer.Texture = null;

		if(BackgroundTexture is not null)
		{ 			
			IconRenderer.Texture = BackgroundTexture;
        }

        SetExlusiveTypeTooltip();
    }

    public void OnGuiInput(InputEvent @event)
	{
		if(@event is InputEventMouseButton mouseEvent)
		{
			if(mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				
				var args = new InventorySlotClickEventArgs()
				{
					SlotIndex = GetIndex(),
					ParentInterface = ParentInterface,
					EquipmentSlotType = EquipmentSlotType,
					ExclusiveType = ExclusiveType
                };

                InventoryInterface.Instance.InventorySlotClicked(args);
            }
        }
	}

	private void SetExlusiveTypeTooltip()
	{
		if(ExclusiveType is not ItemType.Item)
		{
			TooltipText = ExclusiveType.ToString() + " exclusive slot";
		}
    }

	private void ClearExclusiveTypeTooltip()
	{
		TooltipText = string.Empty;
    }
}
