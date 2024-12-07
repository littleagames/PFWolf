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