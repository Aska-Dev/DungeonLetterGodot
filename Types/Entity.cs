using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

public class Components
{
    private List<Component> _components = new List<Component>();

    public Components(Node node)
    {
        Init(node);
    }

    public T? Get<T>(string? name = null) where T : Component
    {
        if (name != null)
        {
            return _components.FirstOrDefault(c => c is T && c.Name == name) as T;
        }
        else
        {
            return _components.FirstOrDefault(c => c is T) as T;
        }
    }

    private void Init(Node node)
    {
        var children = node.GetChildren();
        
        foreach (var child in children)
        {
            if (child is Component component)
            {
                _components.Add(component);
            }
        }
    }
}

public interface IEntity
{
    public Components Components { get; set; }
}
