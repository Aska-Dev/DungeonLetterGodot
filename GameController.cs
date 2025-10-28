using DungeonLetter.Common;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLetter;

public partial class GameController : Node
{
    public static GameController Instance { get; private set; }

    public PlayerUi PlayerUi { get; set; }

    public override void _Ready()
    {
        Instance = this;

        // Set the mouse to captured
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed(Inputs.UiEscape))
        {
            if(UiEventBus.Instance.IsUiOpen)
            {
                UiEventBus.Instance.CloseUi();
                return;
            }

            if (Input.MouseMode == Input.MouseModeEnum.Captured)
            {
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
            else if (Input.MouseMode == Input.MouseModeEnum.Visible)
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
        }
    }
}
