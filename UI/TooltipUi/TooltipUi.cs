using Godot;
using System;

public partial class TooltipUi : PanelContainer
{
    private bool _initInvis = false;
    private Vector2 _offset = new(10, 10);

    public override void _Ready()
    {
        UiEventBus.Instance.OnItemTooltipToggle += Toggle;
    }

    public override void _ExitTree()
    {
        UiEventBus.Instance.OnItemTooltipToggle -= Toggle;
    }

    public override void _Process(double delta)
    {
        if(!_initInvis)
        {
            Hide();
            _initInvis = true;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if(Visible && @event is InputEventMouseMotion)
        {
            GlobalPosition = GetGlobalMousePosition() + _offset;
        }
    }

    public void SetContent(Item item)
    {
        var richTextLabel = GetNode<RichTextLabel>("Content");
        richTextLabel.Text = $"{item.Name}\n[hr]\n[color=dim_gray][i]{GetTypeString(item.Type)}[/i][/color]\n{item.Description}";
    }

    public async void Toggle(bool on, Item? item)
    {
        if(on && item is not null)
        {
            Show();
            SetContent(item);
        }
        else
        {
            Hide();
        }
    }

    private string GetTypeString(ItemType type)
    {
        return type switch
        {
            ItemType.Consumable => "Consumable",
            ItemType.Headgear => "Head",
            ItemType.BodyArmor => "Body",
            ItemType.LegArmor => "Legs",
            ItemType.Boots => "Boots",
            ItemType.MainHand => "Main Hand",
            ItemType.OffHand => "Off Hand",
            _ => "Item"
        };
    }
}
