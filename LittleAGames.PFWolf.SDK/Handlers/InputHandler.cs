namespace LittleAGames.PFWolf.SDK.Handlers;

public class InputHandler
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