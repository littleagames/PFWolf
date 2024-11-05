namespace LittleAGames.PFWolf.SDK.Components;

public class Background : Rectangle
{
    private byte _color;
    private readonly int _x;
    private readonly int _y;
    private readonly int _height;
    private readonly int _width;

    public Background(byte color) // TODO: Make this a "Color" strongly typed
        : base(0, 0, 640, 400, color) // Maybe a "ScreenWidth.Max", and "ScreenHeight.Max" type, and those types translate to "whatever screenwidth and screenheight are in the system because I don't know it here"
    {
    }
}