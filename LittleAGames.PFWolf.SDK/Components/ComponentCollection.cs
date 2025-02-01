namespace LittleAGames.PFWolf.SDK.Components;

public class ComponentCollection
{
    private readonly HashSet<Component> _components = [];

    public void Add<T>(T component) where T : Component
    {
        if (component == null)
            throw new Exception("Cannot add null component");
        _components.Add(component);
    }
    
    public void Add<T>(IEnumerable<T> components) where T : Component
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