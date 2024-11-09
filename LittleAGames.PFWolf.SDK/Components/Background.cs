namespace LittleAGames.PFWolf.SDK.Components;

public class Background : Rectangle
{
    private Background(byte color) // TODO: Make this a "Color" strongly typed
        : base(0, 0, 320, 200, color) // Maybe a "ScreenWidth.Max", and "ScreenHeight.Max" type, and those types translate to "whatever screenwidth and screenheight are in the system because I don't know it here"
    {
    }
    
    public static Background Create(byte color)
        => new Background(color);
}