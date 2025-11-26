using DungeonLetter.Common;
using Godot;
using System;

public enum PlayerHandConfigs
{
    Primary,
    Secondary,
    Consumable,
    Nothing
}

public partial class PlayerHands : Node3D
{
	[ExportCategory("Dependencies")]
    [Export] public required PlayerEquipmentComponent EquipmentComponent { get; set; }
    [Export] public required PlayerActionControllerComponent ActionController { get; set; }
    [Export] public required PlayerHand MainHand { get; set; }
    [Export] public required PlayerHand OffHand { get; set; }

    public HandConfig PrimaryConfig { get; set; } = new HandConfig();
    public HandConfig SecondaryConfig { get; set; } = new HandConfig();
    public PlayerHandConfigs ActiveConfig { get; set; } = PlayerHandConfigs.Primary;

    public override void _Ready()
    {
        InventoryInterface.Instance.OnConsumableSelected += SelectConsumable;
    }

    public override void _Input(InputEvent @event)  
    {
        if(ActionController.IsPerformingAction)
        {
            return;
        }

        if(@event.IsActionPressed(Inputs.LoadPrimaryHandConfig) && ActiveConfig != PlayerHandConfigs.Primary)
        {
            LoadConfig(PrimaryConfig, PlayerHandConfigs.Primary);
            //GD.Print("Loaded Primary Hand Config");
        }
        else if(@event.IsActionPressed(Inputs.LoadSecondaryHandConfig) && ActiveConfig != PlayerHandConfigs.Secondary)
        {
            LoadConfig(SecondaryConfig, PlayerHandConfigs.Secondary);
            //GD.Print("Loaded Secondary Hand Config");
        }
    }

    public void LoadConfig(HandConfig config, PlayerHandConfigs newActiveConfig)
    {
        SetConfig(newActiveConfig);

        if (config.MainHandItem is null)
        {
            MainHand.ClearHand();
        }
        else
        {
            MainHand.SetItem(config.MainHandItem);
        }

        if(config.OffHandItem is null)
        {
            OffHand.ClearHand();
        }
        else
        {
            OffHand.SetItem(config.OffHandItem);
        }
    }

    public void SelectConsumable(int slotIndex)
    {
        MainHand.ClearHand();
        OffHand.ClearHand();

        var consumable = EquipmentComponent.Consumables[slotIndex];
        if(consumable is not null)
        {
            MainHand.SetItem(consumable);
            SetConfig(PlayerHandConfigs.Consumable);
        }
        else
        {
            SetConfig(PlayerHandConfigs.Nothing);
        }
    }

    private void SetConfig(PlayerHandConfigs config)
    {
        ActiveConfig = config;
        InventoryInterface.Instance.PlayerHandConfigChanged(new PlayerHandConfigChangeEventArgs { ActiveConfig = config });
    }
}

public class HandConfig
{
    public HoldableEquipment? MainHandItem { get; set; } = null;
    public HoldableEquipment? OffHandItem { get; set; } = null;
}
