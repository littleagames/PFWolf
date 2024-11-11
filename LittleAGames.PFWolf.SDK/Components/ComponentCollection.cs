namespace LittleAGames.PFWolf.SDK.Components;

public class ComponentCollection
{
    private readonly HashSet<Component> _components = [];

    public void Add(Component component)
    {
        _components.Add(component);
    }
    
    public void Add(HashSet<Component> components)
    {
        foreach(var component in components)
            _components.Add(component);
    }
    
    public void Add(ComponentCollection components)
    {
        Add(components.GetComponents());
    }

    public HashSet<Component> GetComponents()
    {
        return _components;
    }
}