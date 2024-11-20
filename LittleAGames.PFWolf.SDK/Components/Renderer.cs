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

public class AutoMapRenderer : Renderer
{
    private readonly Camera _camera;
    private readonly Map _map;

    private AutoMapRenderer(Camera camera, Map map)
    {
        _camera = camera;
        _map = map;
        Scale = 16.0f;
    }

    public static AutoMapRenderer Create(Camera camera, Map map)
        => new(camera, map);

    public override byte[] Render(int width, int height)
    {
        var graphic = new byte[width * height];
        
        var mapHeight = _map.Walls.GetLength(1);
        var mapWidth = _map.Walls.GetLength(0);

        var scaleWidth = (int)(mapWidth * Scale);
        var scaleHeight = (int)(mapHeight * Scale);
        
        // TODO: Focus point on map (player, 29,57)
        // Get distance left, right, top, bottom?
        // TODO: Get map size * scale
        // Find bounds (what map pixel to scaled pixel, to graphic end
        
        // Fill in the color/gfx etc
        
        // TODO: Build graphic based on the map
        // Maybe its mapW * gfxSize, scale
        // For now, 1 pixel is wall
        for (var y = 0; y < height; y++)
        {
            if (y >= scaleHeight)
                continue;
            
            for (var x = 0; x < width; x++)
            {
                if (x >= scaleWidth)
                    continue;
                
                var originalX = (int) (x / Scale);
                var originalY = (int) (y / Scale);

                // TODO: How do I get asset raw data here?
                var wall = _map.Walls[originalX, originalY];
                if (wall != null && wall.North != null)
                {
                    var gfxX =(int)((x - (Scale*originalX)) * 64/Scale); // TODO: Replace 64 with graphic width/height
                    var gfxY =(int)((y - (Scale*originalY)) * 64/Scale);
                    graphic[y * width + x] = wall.North[gfxX * 64 + gfxY]; // TODO: Raw data is rotated for easier vertical line rendering in raycast
                }
                else
                {
                    graphic[y * width + x] = 0;
                }
            }
        }
        return graphic; // This could be a lot larger than this
    }
}