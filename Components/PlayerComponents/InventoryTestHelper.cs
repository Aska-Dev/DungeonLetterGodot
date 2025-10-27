using Godot;
using System;

/// <summary>
/// Helper class to add test items to the inventory for demonstration
/// </summary>
public partial class InventoryTestHelper : Node
{
    [Export] public InventoryComponent Inventory { get; set; }

    public override void _Ready()
    {
        if (Inventory == null)
        {
            GD.PrintErr("InventoryTestHelper: No inventory assigned!");
            return;
        }

        // Wait a frame for everything to initialize
        CallDeferred(nameof(AddTestItems));
    }

    private void AddTestItems()
    {
        // Add some test items
        AddTestItem("Sword", "res://path/to/sword_icon.png");
        AddTestItem("Shield", "res://path/to/shield_icon.png");
        AddTestItem("Potion", "res://path/to/potion_icon.png");
        AddTestItem("Key", "res://path/to/key_icon.png");
        AddTestItem("Bow", "res://path/to/bow_icon.png");
    }

    private void AddTestItem(string name, string iconPath)
    {
        var item = new Item
        {
            Name = name,
            Model = new ItemModel { Path = iconPath }
        };

        if (!Inventory.AddItem(item))
        {
            GD.Print($"Could not add {name} - inventory full!");
        }
    }

    public override void _Input(InputEvent @event)
    {
        // Press 'P' to add a random test item
        if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.P)
        {
            var randomItems = new[] { "Sword", "Axe", "Potion", "Gold", "Diamond" };
            var randomName = randomItems[GD.RandRange(0, randomItems.Length - 1)];
            AddTestItem(randomName, "res://icon.svg"); // Use default Godot icon as fallback
            GD.Print($"Added test item: {randomName}");
        }
    }
}
