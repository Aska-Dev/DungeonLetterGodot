using DungeonLetter.Common;
using Godot;

public partial class Enemy : CharacterBody3D, IEntity
{
    public Components Components { get; set; } = null!;

	public override void _Ready()
	{
		Components = new Components(this);
    }
}