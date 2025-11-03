using Godot;
using System;
using System.Dynamic;

public partial class GrabbedSlotDisplayUi : PanelContainer
{
	[Export]
	public required TextureRect IconRenderer { get; set; }

    public InventorySlot? Slot { get; set; }

    public override void _Ready()
    {
		Visible = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if(Visible)
		{
			GlobalPosition = GetGlobalMousePosition() + new Vector2(5, 5);
        }
    }

	public void UpdateDisplay(InventorySlot slot)
	{
        Slot = slot;

        IconRenderer.Texture = Slot.Item!.Icon;
        Visible = true;
    }

    public void ClearDisplay()
    {
        Visible = false;
        Slot = null;
    }
}
