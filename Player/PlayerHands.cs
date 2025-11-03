using DungeonLetter.Common;
using Godot;
using System;

public enum PlayerHandConfigs
{
    Primary,
    Secondary
}

public partial class PlayerHands : Node3D
{
	[ExportCategory("Dependencies")]
    [Export] public required PlayerHand MainHand { get; set; }

    public HandConfig PrimaryConfig { get; set; } = new HandConfig();
    public HandConfig SecondaryConfig { get; set; } = new HandConfig();
    public PlayerHandConfigs ActiveConfig { get; set; } = PlayerHandConfigs.Primary;

    public override void _Input(InputEvent @event)  
    {
        if(@event.IsActionPressed(Inputs.LoadPrimaryHandConfig) && ActiveConfig != PlayerHandConfigs.Primary)
        {
            LoadConfig(PrimaryConfig, PlayerHandConfigs.Primary);
            GD.Print("Loaded Primary Hand Config");
        }
        else if(@event.IsActionPressed(Inputs.LoadSecondaryHandConfig) && ActiveConfig != PlayerHandConfigs.Secondary)
        {
            LoadConfig(SecondaryConfig, PlayerHandConfigs.Secondary);
            GD.Print("Loaded Secondary Hand Config");
        }


    }

    public void LoadConfig(HandConfig config, PlayerHandConfigs newActiveConfig)
    {
        ActiveConfig = newActiveConfig;
        InventoryInterface.Instance.PlayerHandConfigChanged(new PlayerHandConfigChangeEventArgs()
        {
            ActiveConfig = ActiveConfig
        });

        if (config.MainHandItem is null)
        {
            GD.Print("PlayerHands: Clearing Main Hand Item");
            MainHand.ClearHand();
        }
        else
        {
            MainHand.EquipItem(config.MainHandItem);
        }
    }
}

public class HandConfig
{
    public Holdable? MainHandItem { get; set; } = null;
    public Holdable? OffHandItem { get; set; } = null;
}
