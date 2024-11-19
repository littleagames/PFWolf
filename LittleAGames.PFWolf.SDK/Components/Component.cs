namespace LittleAGames.PFWolf.SDK.Components;

public abstract class Component
{
    public ComponentCollection Children { get; } = new();
    
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
    // TODO: X and Y? Width and Height? Do all render components have this?
    public bool Hidden { get; set; } = false;
}

public abstract class MapComponent : Component
{
}