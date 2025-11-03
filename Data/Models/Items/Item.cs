using DungeonLetter.Common;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ExclusiveItemId
{
    None,
    MainHand,
    OffHand,
    HeadGear,
    ChestGear,
    LegGear,
    Boots
}

[GlobalClass]
public partial class Item : Resource
{
    [ExportCategory("Info")]
    [Export] public required string Name { get; set; }
    [Export] public required string Description { get; set; }
	[Export] public required CompressedTexture2D Icon { get; set; }

    [ExportCategory("Special")]
    [Export] public ExclusiveItemId ExclusiveItemId { get; set; } = ExclusiveItemId.None;
}