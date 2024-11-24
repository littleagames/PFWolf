namespace LittleAGames.PFWolf.SDK.Components;

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

        var scaledMapWidth = (int)(mapWidth * Scale);
        var scaledMapHeight = (int)(mapHeight * Scale);
        
        // TODO: Focus point on map (player, 29,57)
        // _camera.Position
        var centerX = width / 2;
        var centerY = height / 2;

        var mapCenterX = (int)(_camera.Position.X * Scale);
        var mapCenterY = (int)(_camera.Position.Y * Scale);
        
        // TODO: Calculate left bound
        var topBound = mapCenterY - centerY;
        var bottomBound = mapCenterY + centerY;
        var leftBound = mapCenterX - centerX;
        var rightBound = mapCenterX + centerX;
        
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {   
                var originalX = (int) ((leftBound+x) / Scale);
                var originalY = (int) ((topBound+y) / Scale);
                
                if (originalX < 0 || originalX >= _map.Walls.GetLength(0))
                    continue;
                
                if (originalY < 0 || originalY >= _map.Walls.GetLength(1))
                    continue;
                
                var wall = _map.Walls[originalX, originalY];
                if (wall != null && wall.North != null)
                {
                    var gfxX =(int)(((leftBound+x) - (Scale*originalX)) * 64/Scale); // TODO: Replace 64 with graphic width/height
                    var gfxY =(int)(((topBound+y) - (Scale*originalY)) * 64/Scale);
                    if (gfxX < 0 || gfxX >= 64 || gfxY < 0 || gfxY >= 64)
                        continue;
                    graphic[y * width + x] = wall.North[gfxX * 64 + gfxY]; // TODO: Raw data is rotated for easier vertical line rendering in raycast
                    
                    // TODO: FIX right side cuts off on scale decreasing
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