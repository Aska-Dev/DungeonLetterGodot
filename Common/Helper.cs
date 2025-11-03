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
    public static T DeepCopy<T>(T original)
    {
        string jsonString = JsonSerializer.Serialize(original);
        return JsonSerializer.Deserialize<T>(jsonString)!;
    }


}
