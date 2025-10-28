using DungeonLetter.Common;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public partial class Item : Resource
{
	[Export]
	public required ItemModel Model { get; set; }

	[ExportGroup("Info")]

    [Export]
	public required string Name { get; set; }
	[Export]
	public required string Description { get; set; }
	[Export]
	public required AtlasTexture Icon { get; set; }

    [ExportGroup("Animations")]

    [Export]
	public virtual string UseAnimation { get; set; } = "";
	[Export]
	public virtual string IdleAnimation { get; set; } = "";
}
