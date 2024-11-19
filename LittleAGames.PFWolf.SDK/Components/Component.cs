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

public class InputComponent : Component
{
    private readonly HashSet<Keys> _keysPressed = new();

    public bool IsAnyKeyPressed => _keysPressed.Any();

    public void SetKeyDown(Keys keyCode)
    {
        _keysPressed.Add(keyCode);
    }

    public bool IsKeyDown(Keys keyCode) => _keysPressed.Any(x => x == keyCode);

    public void SetKeyUp(Keys keyCode)
    {
        _keysPressed.Remove(keyCode);
    }
    
    public void ClearKeysDown()
    {
        _keysPressed.Clear();
    }
}

public enum Keys
{
    None,
    Up,
    Down,
    Left,
    Right,
    Return,
    Escape
}