using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public partial class InventorySlot : Resource
{
    [Export]
    public Item? Item { get; set; } = null;

    public bool IsEmpty => Item == null;
}
