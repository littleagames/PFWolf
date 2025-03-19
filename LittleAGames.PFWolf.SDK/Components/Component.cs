namespace LittleAGames.PFWolf.SDK.Components;

public abstract class Component : RunnableBase
{
    public ComponentCollection Children { get; } = new();

    public virtual void OnStart()
    {
        
    }

    public virtual void OnPreUpdate()
    {
        
    }
    
    public virtual void OnUpdate()
    {
        
    }

    public virtual void OnDestroy()
    {
        
    }

    public T? FindComponent<T>() where T : Component
    {
        foreach (var child in Children.GetComponents())
        {
            if (child is T component)
            {
                return component;
            }
            
            child.FindComponent<T>();
        }

        return null;
    }
    
    public IEnumerable<T> FindComponents<T>() where T : Component
    {
        foreach (var child in Children.GetComponents())
        {
            if (child is T component)
            {
                yield return component;
            }
            
            child.FindComponent<T>();
        }
    }
}

public abstract class RenderComponent : Component
{
    public bool Hidden { get; set; } = false;
}

public abstract class MapComponent : Component
{
    public int X { get; protected set; }
    public int Y { get; protected set; }
    public int Width { get; init; }
    public int Height { get; init; }
    public byte[] Data { get; init; } = [];
    public PhysicsBox? PhysicsBody = null;
}

public class InputComponent : Component
{
    private readonly HashSet<Keys> _keysPressed = new();

    public bool IsAnyKeyPressed => _keysPressed.Any();

    public void SetKeyDown(Keys keyCode)
    {
        Console.WriteLine($"Key down: {keyCode}");
        _keysPressed.Add(keyCode);
    }

    public bool IsKeyDown(Keys keyCode) => _keysPressed.Any(x => x == keyCode);

    public void SetKeyUp(Keys keyCode)
    {
        Console.WriteLine($"Key up: {keyCode}");
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
    Escape,
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    I,
    J,
    K,
    L,
    M,
    N,
    O,
    P,
    Q,
    R,
    S,
    T,
    U,
    V,
    W,
    X,
    Y,
    Z,
    Equals,
    Minus,
    LeftShift,
    RightShift,
    Tab,
    Tilde,
    Space,
    Backspace
}