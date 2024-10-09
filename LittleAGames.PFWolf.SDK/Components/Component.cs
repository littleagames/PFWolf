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
}

public abstract class Component
{
    public abstract void Draw();
}
