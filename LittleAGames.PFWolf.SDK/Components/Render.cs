namespace LittleAGames.PFWolf.SDK.Components;

public class Render : RenderComponent
{
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }
    public byte[,] Data { get; set; }
    
    protected Render(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Data = new byte[width, height];
    }
    
    public static Render Create(int x, int y, int width, int height)
        => new(x, y, width, height);

    public void Clear()
    {
        // TODO: Verify it hasn't changed length
        Data = new byte[Width, Height];
    }   

    public void DrawLine(int x0, int y0, int x1, int y1, byte color)
    {
        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            if ((uint)x0 < Data.GetLength(0) && (uint)y0 < Data.GetLength(1))
                Data[x0, y0] = color; // Mark the cell as part of the line

            if (x0 == x1 && y0 == y1)
                break;

            int e2 = 2 * err;

            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }
}