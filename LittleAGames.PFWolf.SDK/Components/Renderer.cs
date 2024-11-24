namespace LittleAGames.PFWolf.SDK.Components;

public abstract class Renderer : RenderComponent
{
    public float Scale { get; protected set; }
    
    public abstract byte[] Render(int width, int height);
    
    public void UpdateScale(float scale)
    {
        if (scale < 0)
            Scale = 0;
        else
            Scale = scale;
    }

}