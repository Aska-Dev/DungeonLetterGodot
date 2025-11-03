using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLetter.Common;

public static class Inputs
{
    public const string MoveForward = "player_move_forward";
    public const string MoveBack = "player_move_back";
    public const string MoveLeft = "player_move_left";
    public const string MoveRight = "player_move_right";
    public const string MoveJump = "player_move_jump";

    public const string ActionFirst = "player_action_first";
    public const string ActionSecondary = "player_action_secondary";

    public const string ActionInteract = "player_interaction";
    public const string ActionInventory = "player_toggle_inventory";

    public const string LoadPrimaryHandConfig = "player_load_primary_hand_config";
    public const string LoadSecondaryHandConfig = "player_load_secondary_hand_config";

    public const string UiEscape = "global_ui_cancel";

}
