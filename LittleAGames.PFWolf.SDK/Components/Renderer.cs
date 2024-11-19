namespace LittleAGames.PFWolf.SDK.Components;

public abstract class Renderer : RenderComponent
{
    public abstract byte[] Render(int width, int height);
}

public class AutoMapRenderer : Renderer
{
    private readonly Camera _camera;
    private readonly Map _map;

    private AutoMapRenderer(Camera camera, Map map)
    {
        _camera = camera;
        _map = map;
    }

    public static AutoMapRenderer Create(Camera camera, Map map)
        => new(camera, map);

    public override byte[] Render(int width, int height)
    {
        var graphic = new byte[width * height];
        // TODO: Build graphic based on the map
        // Maybe its mapW * gfxSize, scale
        // For now, 1 pixel is wall
        for (var y = 0; y < height; y++)
        {
            if (y >= _map.Walls.GetLength(1))
                continue;
            
            for (var x = 0; x < width; x++)
            {
                if (x >= _map.Walls.GetLength(0))
                    continue;

                // TODO: How do I get asset raw data here?
                graphic[y * width + x] = (byte)((_map.Walls[x, y] != null) ? 1 : 0);
            }
        }
        return graphic; // This could be a lot larger than this
    }
}