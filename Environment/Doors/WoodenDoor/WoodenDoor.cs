using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class WoodenDoor : Environment
{
    private enum OpenDirection
    { 
        Front,
        Back
    }

    [Signal]
    public delegate void OnOpenFrontEventHandler();
    [Signal]
    public delegate void OnCloseFrontEventHandler();
    [Signal]
    public delegate void OnOpenBackEventHandler();
    [Signal]
    public delegate void OnCloseBackEventHandler();

    [Export]
    public required bool IsOpen { get; set; } = false;

    private OpenDirection openDirection = OpenDirection.Front;

    public void OnInteract()
    {
        var playerInFront = IsPlayerInFront();
        if (playerInFront)
        {
            InteractFront();
        }
        else
        {
            InteractBack();
        }
    }

    public void InteractFront()
    {
        if(IsOpen)
        {
            Close();
        }
        else
        {
            Open(OpenDirection.Back);
        }
    }

    public void InteractBack()
    {
        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open(OpenDirection.Front);
        }
    }

    private void Open(OpenDirection openDir)
    {
        Components.Get<InteractionComponent>()!.SetInteractionMessage("Press [F] to close Door");

        IsOpen = true;
        openDirection = openDir;

        if (openDir == OpenDirection.Front)
        {
            EmitSignal(SignalName.OnOpenFront);
        }
        else
        {
            EmitSignal(SignalName.OnOpenBack);
        }
    }

    private void Close()
    {
        Components.Get<InteractionComponent>()!.SetInteractionMessage("Press [F] to open Door");

        IsOpen = false;

        if (openDirection == OpenDirection.Front)
        {
            EmitSignal(SignalName.OnCloseFront);
        }
        else
        {
            EmitSignal(SignalName.OnCloseBack);
        }
    }

    private bool IsPlayerInFront()
    {
        var globalPlayerPos = GetTree().CurrentScene.GetNode<Node3D>("Player")?.GlobalPosition;
        GD.Print(globalPlayerPos);

        Vector3 local = ToLocal(globalPlayerPos ?? Vector3.Zero);
        return local.Z > 0;
    }
}
