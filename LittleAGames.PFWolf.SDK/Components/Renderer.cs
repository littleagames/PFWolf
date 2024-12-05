namespace LittleAGames.PFWolf.SDK.Components;

public abstract class Renderer : RenderComponent
{
    protected Camera Camera;
    protected int Width;
    protected int Height;
    
    protected Renderer(Camera camera, int width, int height)
    {
        Camera = camera;
        Width = width;
        Height = height;
    }

    public abstract byte[,] Render();
}