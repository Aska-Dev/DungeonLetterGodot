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
	public ItemModel Model { get; set; }
	
	[Export]
	public string Name { get; set; }

	[Export]
	public virtual string UseAnimation { get; set; } = PlayerAnimations.SwordAttack;

	[Export]
	public virtual string IdleAnimation { get; set; } = PlayerAnimations.SwordIdle;
}
