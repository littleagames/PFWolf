﻿namespace LittleAGames.PFWolf.SDK.Components;

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
    public int X { get; init; }
    public int Y { get; init; }
    public int Width => Data.GetLength(0);
    public int Height => Data.GetLength(1);
    public byte[,] Data { get; set; } = new byte[0,0];
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
    D,
    S,
    W,
    Equals,
    Minus,
    LeftShift,
    RightShift,
    Tab
}