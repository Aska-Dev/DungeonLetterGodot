using Godot;
using System;

public partial class InventorySlotUi : PanelContainer
{
	[Export] public required TextureRect IconRenderer { get; set; }

	public void SetItem(Item item)
	{
		IconRenderer.Texture = item.Icon;
    }
}
