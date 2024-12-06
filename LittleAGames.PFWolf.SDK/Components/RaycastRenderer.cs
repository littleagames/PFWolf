namespace LittleAGames.PFWolf.SDK.Components;

public class RaycastRenderer : Renderer
{
    private readonly Map _map;
    
    public RaycastRenderer(Camera camera, Map map, int width, int height) : base(camera, width, height)
    {
        _map = map;
    }
    
    public static RaycastRenderer Create(Camera camera, Map map, int width, int height)
        => new(camera, map, width, height);
    
    public override byte[,] Render()
    {
        var pa = (float)Camera.Angle;
        var result = new byte[Width, Height];
        return result;
    }

}