using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleAGames.PFWolf.SDK.Components;

public class ComponentCollection
{
    private HashSet<Component> _components = new HashSet<Component>();

    public void Add(Component component)
    {
        _components.Add(component);
    }

    public HashSet<Component> GetComponents()
    {
        return _components;
    }
}

public abstract class Component
{
    public virtual void OnStart()
    {
        
    }

    public virtual void OnUpdate()
    {
        
    }

    public virtual void OnDestroy()
    {
        
    }
}

public abstract class RenderComponent : Component
{
}
