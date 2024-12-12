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

    private const int mapShift = 4;
    private const int mapScale = (1<<mapShift);     //map cube size // TODO: This becomes TILEGLOBAL

    private void drawMap2D(byte[,] result)
    {
        byte color;
        int x,y,xOffset,yOffset;
        for(y=0;y<_map.Height;y++)
        {
            for(x=0;x<_map.Width;x++)
            {
                if (_map.Plane[0][y,x] > 0 && _map.Plane[0][y,x] < 90)
                {
                    color = 0x0f; // white?
                }
                else
                {
                    color = 0x00; // black
                }
                
                // TODO: fine
                var shift = 16 - (int)Math.Log2(mapScale);
                xOffset = (((x<<16) - Camera.X + (1<<16)/2) >> shift) + Width/2;
                yOffset = (((y<<16)-Camera.Y + (1<<16)/2) >> shift) + Height/2;
                //xOffset =(x-Camera.TileX)*mapScale + Width/2;
                //yOffset =(y-Camera.TileY)*mapScale + Height/2;
                
                var posLeftTop = ( X:0   +xOffset+1, Y:0   +yOffset+1);  // left top
                var posLeftBottom = ( X:0   +xOffset+1, Y:mapScale+yOffset-1);  // left bottom
                var posRightBottom = ( X:mapScale+xOffset-1, Y:mapScale+yOffset-1);  // right bottom
                var posRightTop = ( X:mapScale+xOffset-1, Y:0   +yOffset+1);  // right top
                
                DrawLine(result, posLeftTop.X, posLeftTop.Y, posLeftBottom.X, posLeftBottom.Y, color);
                DrawLine(result, posLeftBottom.X, posLeftBottom.Y, posRightBottom.X, posRightBottom.Y, color);
                DrawLine(result, posRightBottom.X, posRightBottom.Y, posRightTop.X, posRightTop.Y, color);
                DrawLine(result, posRightTop.X, posRightTop.Y, posLeftTop.X, posLeftTop.Y, color);
            } 
        } 
    }
    
    //------------------------PLAYER------------------------------------------------
    
    private static int FixAng(int a){ if(a>359){ a-=360;} if(a<0){ a+=360;} return a;}
    private static float FixAng(float a) => FixAng((int)a);

    private int px, py;
    private double pa;

    private void drawPlayer2D(byte[,] grid)
    {
        // TODO: Global var "color"
        byte color = 0x20; // red

        var shift = 16 - (int)Math.Log2(mapScale);
        px = (Camera.X >> shift);
        py = (Camera.Y >> shift);
        var playerX = Width / 2 + (mapScale / 2);
        var playerY = Height / 2 + (mapScale / 2);
        pa = Camera.Angle;

        DrawPoint(grid, playerX, playerY, mapScale, color);

        var angleRadians = double.DegreesToRadians(pa);

        var length = mapScale;
        
        // Calculate the new X and Y using trigonometry
        var pdx = (int)(playerX + length * Math.Cos(angleRadians));
        var pdy = (int)(playerY + length * -Math.Sin(angleRadians));
        
        // TODO: Draw a 4px thick line
        DrawLine(grid, playerX, playerY,pdx,pdy, color);
    }

    void drawRays2D(byte[,] grid)
    {
        int r, mx, my, mp, dof, side;
        float vx, vy, rx, ry, ra, xo = 0, yo = 0, disV, disH;

        ra = FixAng((float)pa + 30); //ray set back 30 degrees

        for (r = 0; r < 60; r++) // todo: instead of calculating rays, get each vertical line on the screen, and calc that ray
        {
            //---Vertical--- 
            dof = 0;
            side = 0;
            disV = 100000;
            float Tan = (float)Math.Tan(double.DegreesToRadians(ra));
            if (Math.Cos(double.DegreesToRadians(ra)) > 0.001)
            {
                rx = (((int)px >> mapShift) << mapShift) + mapScale;
                ry = (px - rx) * Tan + py;
                xo = mapScale;
                yo = -xo * Tan;
            } //looking left
            else if (Math.Cos(double.DegreesToRadians(ra)) < -0.001)
            {
                rx = ((px >> mapShift) << mapShift) - 0.0001f;
                ry = (px - rx) * Tan + py;
                xo = -mapScale;
                yo = -xo * Tan;
            } //looking right
            else
            {
                rx = px;
                ry = py;
                dof = 8;
            } //looking up or down. no hit  

            while (dof < 8)
            {
                mx = (int)(rx) >> mapShift;
                my = (int)(ry) >> mapShift;
                if (mx >= 0 && mx < _map.Plane[0].GetLength(1)
                           && my >= 0 && my < _map.Plane[0].GetLength(0)
                           && _map.Plane[0][my,mx] > 0 && _map.Plane[0][my,mx] < 90)
                {
                    dof = 8;
                    var degRad = double.DegreesToRadians(ra);
                    disV = (float)Math.Cos(degRad) * (rx - px) - (float)Math.Sin(degRad) * (ry - py);
                } //hit         
                else
                {
                    rx += xo;
                    ry += yo;
                    dof += 1;
                } //check next horizontal
            }

            vx = rx;
            vy = ry;

            //---Horizontal---
            dof = 0;
            disH = 100000;
            Tan = 1.0f / Tan;
            if (Math.Sin(double.DegreesToRadians(ra)) > 0.001f)
            {
                ry = (((int)py >> mapShift) << mapShift) - 0.0001f;
                rx = (py - ry) * Tan + px;
                yo = -mapScale;
                xo = -yo * Tan;
            } //looking up 
            else if (Math.Sin(double.DegreesToRadians(ra)) < -0.001f)
            {
                ry = (((int)py >> mapShift) << mapShift) + mapScale;
                rx = (py - ry) * Tan + px;
                yo = mapScale;
                xo = -yo * Tan;
            } //looking down
            else
            {
                rx = px;
                ry = py;
                dof = 8;
            } //looking straight left or right

            while (dof < 8)
            {
                mx = (int)(rx) >> mapShift;
                my = (int)(ry) >> mapShift;
                if (mx >= 0 && mx < _map.Plane[0].GetLength(1)
                            && my >= 0 && my < _map.Plane[0].GetLength(0)
                            && _map.Plane[0][my, mx] > 0 && _map.Plane[0][my, mx] < 90)
                {
                    dof = 8;
                    disH = (float)Math.Cos(double.DegreesToRadians(ra)) * (rx - px) - (float)Math.Sin(double.DegreesToRadians(ra)) * (ry - py);
                } //hit         
                else
                {
                    rx += xo;
                    ry += yo;
                    dof += 1;
                } //check next horizontal
            }

            byte color = 0x65; // green?
            if (disV < disH)
            {
                rx = vx;
                ry = vy;
                disH = disV;

                color = 0x6a;
            } //horizontal hit first

            // TODO: 2px line
            var playerX = Width / 2 + (mapScale / 2);
            var playerY = Height / 2 + (mapScale / 2);
            var destX = (int)(rx - px + playerX);
            var destY = (int) (ry - py + playerY);
            DrawLine(grid, playerX, playerY, destX, destY, color);

            // int ca = (int)FixAng((float)(pa - ra));
            // disH = disH * (float)Math.Cos(degToRad(ca)); //fix fisheye 
            // int lineH = (int)((mapScale * 320) / (disH));
            // if (lineH > 320)
            // {
            //     lineH = 320;
            // } //line height and limit
            //
            // int lineOff = 160 - (lineH >> 1); //line offset

            //glLineWidth(8);
            //glBegin(GL_LINES);

            // 8px thick line
            // SDL.SDL_RenderDrawLine(renderer, r*8-3+530,lineOff, r*8-3+530,lineOff+lineH);`
            // SDL.SDL_RenderDrawLine(renderer, r*8-2+530,lineOff, r*8-2+530,lineOff+lineH);
            // SDL.SDL_RenderDrawLine(renderer, r*8-1+530,lineOff, r*8-1+530,lineOff+lineH);
            // SDL.SDL_RenderDrawLine(renderer, r*8+530,lineOff, r*8+530,lineOff+lineH);
            // SDL.SDL_RenderDrawLine(renderer, r*8+1+530,lineOff, r*8+1+530,lineOff+lineH);
            // SDL.SDL_RenderDrawLine(renderer, r*8+2+530,lineOff, r*8+2+530,lineOff+lineH);
            // SDL.SDL_RenderDrawLine(renderer, r*8+3+530,lineOff, r*8+3+530,lineOff+lineH);
            // SDL.SDL_RenderDrawLine(renderer, r*8+4+530,lineOff, r*8+4+530,lineOff+lineH);

            ra = FixAng(ra - 1); //go to next ray
        }
    } //-----------------------------------------------------------------------------

    public override byte[,] Render()
    {
        var result = new byte[Width, Height];
        result.Fill((byte)0x19);
        
        // draw things
        drawMap2D(result);
        drawPlayer2D(result);
        drawRays2D(result);
        
        return result;
    }

    private static void DrawPoint(byte[,] grid, int x, int y, int size, byte color)
    {
        for (var sX = x-(size / 2); sX < x+(size / 2); sX++)
            for (var sY = y - (size / 2); sY < y+(size / 2); sY++)
            {
                if (sX < 0 || sX >= grid.GetLength(0) || sY < 0 || sY >= grid.GetLength(1))
                    continue;

                grid[sX, sY] = color;
            }
    }
    
    private static void DrawLine(byte[,] grid, int x1, int y1, int x2, int y2, byte color)
    {
        int dx = Math.Abs(x2 - x1);
        int dy = Math.Abs(y2 - y1);
        int sx = (x1 < x2) ? 1 : -1;
        int sy = (y1 < y2) ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            // Mark the point on the grid
            if (x1 >= 0 && x1 < grid.GetLength(0) && y1 >= 0 && y1 < grid.GetLength(1))
            {
                grid[x1,y1] = color;  // Mark this cell with '#' to indicate the line
            }

            // If the endpoint is reached, break the loop
            if (x1 == x2 && y1 == y2) break;

            int e2 = err * 2;
            if (e2 > -dy)
            {
                err -= dy;
                x1 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y1 += sy;
            }
        }
    }
}