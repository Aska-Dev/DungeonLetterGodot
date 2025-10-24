using Godot;
using System;

public partial class WoodenDoor : Environment
{
    private bool _open = false;
    [Export]
    public bool Open
    {
       get { return _open; }
       set
       { 
            _open = value;
            OnOpenChange();
       }
    }

    public override void _Ready()
    {
        base._Ready();
        OnOpenChange();
    }

    public void Interact()
    {
        Open = !Open;
    }

    private void OnOpenChange()
    {
        if(Open)
        {
            //animator.SetState("open");
            //SetCollission(false);
        }
        else
        {
            //animator.SetState("close");
            //SetCollission(false);
        }
    }
}
