namespace LittleAGames.PFWolf.SDK.Components;

public class Rectangle : RenderComponent
{
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }
    public byte Color { get; }

    protected Rectangle(int x, int y, int width, int height, byte color)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Color = color;
    }
    
    public static Rectangle Create(int x, int y, int width, int height, byte color)
        => new(x, y, width, height, color);
}