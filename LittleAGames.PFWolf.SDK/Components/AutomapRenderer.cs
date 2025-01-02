namespace LittleAGames.PFWolf.SDK.Components;

public class AutoMapRenderer : Renderer
{
    private readonly Map _map;
    private const int ScaleX = 16;
    private const int ScaleY = 16;

    public AutoMapRenderer(Camera camera, Map map, int width, int height) : base(camera, width, height)
    {
        _map = map;
    }

    public static AutoMapRenderer Create(Camera camera, Map map, int width, int height)
        => new(camera, map, width, height);

    public override byte[,] Render()
    {
        var result = new byte[Width, Height];
        result.Fill((byte)0x19);

        var pa = (float)Camera.Angle;

        var offsetX = Width/2 - Camera.TileX*ScaleX; // TODO: Takes the viewport center, and finds where the map starts (it'll most likely be negative)

        var offsetY = Height/2 - Camera.TileY*ScaleY; // TODO: Takes the viewport center, and finds where the map starts (it'll most likely be negative)

        // Walls
        for (var y = 0; y < _map.Height; y++)
        {
            for (var x = 0; x < _map.Width; x++)
            {
                var wallId = _map.Plane[0][y, x];
                if (wallId <= 0 || wallId >= 90)
                    continue;
                
                var wall = _map.Walls[wallId];
                for (var scx = 0; scx < ScaleX; scx++)
                for (var scy = 0; scy < ScaleY; scy++)
                {
                    var drawX = x * ScaleX + scx + offsetX;
                    var drawY = y * ScaleY + scy + offsetY;
                    if (drawX < 0 || drawX >= Width || drawY < 0 || drawY >= Height)
                        continue;

                    var gfxX = scx / (wall.Width / ScaleX);
                    var gfxY = scy / (wall.Height / ScaleY);

                    result[drawX, drawY] = wall.Data[gfxX, gfxY];
                }
            }
        }

        foreach (var wall in _map.FindComponents<Wall>())
        {
            var tileX = wall.X;
            var tileY = wall.Y;

        }

        // Player
        for (var scx = 0; scx < ScaleX; scx++)
            for (var scy = 0; scy < ScaleY; scy++)
            {
                var drawX = Camera.TileX * ScaleX+scx+offsetX;
                var drawY = Camera.TileY * ScaleY+scy+offsetY;
                if (drawX < 0 || drawX >= Width || drawY < 0 || drawY >= Height)
                    continue;
                
                result[drawX, drawY] = 0x40;
            }

        double angleRad = pa * Math.PI / 180;
        DrawLine(result, Width / 2, Height / 2, angleRad, 12, 0x40);

        return result;
    }


    private static void DrawLine(byte[,] grid, int startX, int startY, double angle, int length, byte color)
    {
        int x = startX;
        int y = startY;

        // Calculate the direction in x and y using the angle
        double dx = Math.Cos(angle);
        double dy = -Math.Sin(angle);

        for (int i = 0; i < length; i++)
        {
            // Plot the point in the 2D array (make sure it's within bounds)
            if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
            {
                grid[x, y] = color;
            }

            // Update x and y for the next point
            x = (int)(startX + i * dx);
            y = (int)(startY + i * dy);
        }
    }
}