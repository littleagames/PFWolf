namespace LittleAGames.PFWolf.SDK.Components;

public class ViewPort : RenderComponent
{
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }
    public Renderer Renderer { get; }

    private ViewPort(int x, int y, int width, int height, Renderer renderer)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Renderer = renderer;
    }
    
    public static ViewPort Create(int x, int y, int width, int height, Renderer renderer)
        => new(x, y, width, height, renderer);

    public byte[,] Render()
    {
        // Crop the render
        return Renderer.Render();
    }
}