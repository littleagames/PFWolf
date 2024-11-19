namespace LittleAGames.PFWolf.SDK.Components;

public class ViewPort(int x, int y, int width, int height, Renderer renderer) : RenderComponent
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public int Width { get; } = width;
    public int Height { get; } = height;
    
    public Renderer Renderer { get; } = renderer;

    public static ViewPort Create(int x, int y, int width, int height, Renderer renderer)
        => new(x, y, width, height, renderer);

    public byte[] Render()
    {
        // Crop the render
        return Renderer.Render(Width, Height);
    }
}