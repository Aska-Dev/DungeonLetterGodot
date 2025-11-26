using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DungeonLetter.Common;

public static class Helper
{
    public static T? GetFirstNodeOrDefault<T> (Node rootNode) where T : Node
    {
        foreach (Node child in rootNode.GetChildren())
        {
            if (child is T typedChild)
            {
                return typedChild;
            }
        }

        return null;
    }
}
