using Godot;
using System;

public partial class ConsumableHotbarSlot : PanelContainer
{
    [Export] public EquipmentSlotType ConsumableSlot { get; set; }
    public TextureRect IconRenderer { get; set; }

    public override void _Ready()
    {
        IconRenderer = GetNode<TextureRect>("MarginContainer/TextureRect");

        var children = GetChildren();
    }

    public void SetIcon(Texture2D? texture)
    {
        IconRenderer.Texture = texture;
    }

    public bool HasIcon()
    {
        return IconRenderer.Texture is not null;
    }

    public void Select()
    {
        var color = Modulate;
        color.A = 1f;
        Modulate = color;
    }

    public void Deselect()
    {
        var color = Modulate;
        color.A = 0.5f;
        Modulate = color;
    }

}
