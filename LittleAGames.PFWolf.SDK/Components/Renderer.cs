namespace LittleAGames.PFWolf.SDK.Components;

public abstract class Renderer : RenderComponent
{
    protected Camera? Camera = null;
    
    /// <summary>
    /// Number of pixels per tile
    /// </summary>
    public int Scale { get; protected set; }
    
    public abstract byte[] Render(int width, int height);
    
    public void UpdateScale(int scale)
    {
        if (scale < 0)
            Scale = 0;
        else
            Scale = scale;
    }

    public void SetCamera(Camera camera)
    {
        Camera = camera;
    }
}