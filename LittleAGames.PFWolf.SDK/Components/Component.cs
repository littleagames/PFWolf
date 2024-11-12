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
    public bool Hidden { get; set; } = false;
}
