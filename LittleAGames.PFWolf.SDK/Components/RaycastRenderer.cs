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
        var result = new byte[Width, Height];
        result.Fill((byte)0x19);

        for (var pixx = 0; pixx < Width; pixx++)
        {
            
        }
        
        return result;
    }
}