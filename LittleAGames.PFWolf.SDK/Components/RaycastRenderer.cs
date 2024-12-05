namespace LittleAGames.PFWolf.SDK.Components;

public class RaycastRenderer : Renderer
{
    private readonly Map _map;

    private int mapS = 64;
    
    public RaycastRenderer(Camera camera, Map map, int width, int height) : base(camera, width, height)
    {
        _map = map;
    }
    
    public static RaycastRenderer Create(Camera camera, Map map, int width, int height)
        => new(camera, map, width, height);
    
    public override byte[,] Render()
    {
        pa = (float)Camera.Angle;
        var result = new byte[Width, Height];
        var mapSide = DrawMap2D(Height,Height);
        //var raycastSide = DrawMap3D(Height,Height);

        var offsetX = Height+(16)*2;
        var offsetY = 0;
        
        // Draw map rectangle onto viewport rectangle
        for (var y = 0; y < mapSide.GetLength(1); y++)
        {
            for (var x = 0; x < mapSide.GetLength(0); x++)
            {
                result[x,y] = mapSide[x,y];
            }
        }
        
        // Draw map rectangle onto viewport rectangle
        // for (var y = 0; y < raycastSide.GetLength(0); y++)
        // {
        //     for (var x = 0; x < raycastSide.GetLength(1); x++)
        //     {
        //         result[x+offsetX,y+offsetY] = raycastSide[x,y];
        //     }
        // }
        return result;
    }

    private byte[,] DrawMap2D(int width, int height)
    {
        var result = new byte[_map.Width, _map.Height];
        
        foreach (var wall in _map.FindComponents<Wall>())
        {
            var mapTileX = wall.X;
            var mapTileY = wall.Y;
            
            result[mapTileX, mapTileY] = 0x02;
        }
        
        foreach (var actor in _map.FindComponents<Actor>())
        {
            var mapTileX = actor.Position.X;
            var mapTileY = actor.Position.Y;
            
            result[mapTileX, mapTileY] = 0x04;
        }

        return StretchArray(result, width, height);
    }
    
    private static float degToRad(int a) { return (float)(a*Math.PI/180.0f);}
    private static int FixAng(int a){ if(a>359){ a-=360;} if(a<0){ a+=360;} return a;}
    float px,py,pdx,pdy,pa;
    private static float distance(int ax, int ay,int bx,int by,int ang){ return (float)(Math.Cos(degToRad(ang))*(bx-ax)-Math.Sin(degToRad(ang))*(by-ay));}

    private bool isWallTile(int mapX, int mapY)
    {
        var wall = _map.FindComponents<Wall>().FirstOrDefault(x => x.X == mapX && x.Y == mapY);
        return wall != null;
    }
    
    private byte[,] DrawMap3D(int width, int height)
    {
        var result = new byte[width, height];
        var mapX = _map.Width;
        var mapY = _map.Height;

        int r,mx,my,mp,dof,side;
        float vx,vy,rx,ry;
        int ra = 0;
        float xo = 0,yo = 0,disV = 0,disH = 0;

        ra=FixAng((int)pa+30);                                                              //ray set back 30 degrees

        for (r = 0; r < width; r++)
        {
            //---Vertical--- 
            dof = 0;
            side = 0;
            disV = 100000;
            double Tan = Math.Tan(degToRad(ra));
            if (Math.Cos(degToRad(ra)) > 0.001f)
            {
                rx = (((int)px >> 6) << 6) + 64;
                ry = (float)((px - rx) * Tan + py);
                xo = 64;
                yo = (float)(-xo * Tan);
            } //looking left
            else if (Math.Cos(degToRad(ra)) < -0.001f)
            {
                rx = (((int)px >> 6) << 6) - 0.0001f;
                ry = (float)((px - rx) * Tan + py);
                xo = -64;
                yo = (float)(-xo * Tan);
            } //looking right
            else
            {
                rx = px;
                ry = py;
                dof = 8;
            }

            while (dof < 8)
            {
                mx = (int)(rx) >> 6;
                my = (int)(ry) >> 6;
                mp = my * mapX + mx;
                if (mp > 0 && mp < mapX * mapY && isWallTile(mapX, mapY))
                {
                    dof = 8;
                    disV = (float)(Math.Cos(degToRad(ra)) * (rx - px) - Math.Sin(degToRad(ra)) * (ry - py));
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
            Tan = 1.0 / Tan;
            if (Math.Sin(degToRad(ra)) > 0.001f)
            {
                ry = (((int)py >> 6) << 6) - 0.0001f;
                rx = (float)((py - ry) * Tan + px);
                yo = -64;
                xo = (float)(-yo * Tan);
            } //looking up 
            else if (Math.Sin(degToRad(ra)) < -0.001)
            {
                ry = (((int)py >> 6) << 6) + 64;
                rx = (float)((py - ry) * Tan + px);
                yo = 64;
                xo = (float)(-yo * Tan);
            } //looking down
            else
            {
                rx = px;
                ry = py;
                dof = 8;
            } //looking straight left or right

            while (dof < 8)
            {
                mx = (int)(rx) >> 6;
                my = (int)(ry) >> 6;
                mp = my * mapX + mx;
                if (mp > 0 && mp < mapX * mapY && isWallTile(mapX, mapY))
                {
                    dof = 8;
                    disH = (float)(Math.Cos(degToRad(ra)) * (rx - px) - Math.Sin(degToRad(ra)) * (ry - py));
                } //hit         
                else
                {
                    rx += xo;
                    ry += yo;
                    dof += 1;
                } //check next horizontal
            }

            //glColor3f(0,0.8,0);
            byte color = 0x44;
            if (disV < disH)
            {
                rx = vx;
                ry = vy;
                disH = disV;
                color = 0x20;
            } //horizontal hit first
            
            //glLineWidth(2); glBegin(GL_LINES); glVertex2i(px,py); glVertex2i(rx,ry); glEnd();//draw 2D ray
            //DrawLine(result, (int)px, (int)py, (int)rx, (int)ry, color);

            int ca = FixAng((int)(pa - ra));
            disH = (float)(disH * Math.Cos(degToRad(ca))); //fix fisheye 
            int lineH = (int)((mapS * height) / (disH));
            if (lineH > height)
            {
                lineH = height;
            } //line height and limit

            int lineOff = (height/2) - (lineH >> 1); //line offset

            DrawLine(result, r, lineOff, r, lineOff + lineH, color);

            ra = FixAng(ra - 1);
        }

        return result;
    }
    
    // Bresenham's Line Algorithm to draw a line between two points
    static void DrawLine(byte[,] grid, int x0, int y0, int x1, int y1, byte color)
    {
        var dx = Math.Abs(x1 - x0);
        var dy = Math.Abs(y1 - y0);
        var sx = (x0 < x1) ? 1 : -1;
        var sy = (y0 < y1) ? 1 : -1;
        var err = dx - dy;

        while (true)
        {
            // Plot the point (mark it with 'X')
            if (x0 >= 0 && x0 < grid.GetLength(1) && y0 >= 0 && y0 < grid.GetLength(0))
            {
                grid[y0, x0] = color; // Mark the grid cell
            }

            // If the end point is reached, break the loop
            if (x0 == x1 && y0 == y1)
                break;

            var e2 = err * 2;
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
    
    private static byte[,] StretchArray(byte[,] original, int targetRows, int targetCols)
    {
        var originalRows = original.GetLength(0);
        var originalCols = original.GetLength(1);

        // Create a new 2D array of the target size
        var newArray = new byte[targetRows, targetCols];

        // Calculate scaling factors for row and column indices
        var rowScale = (double)(originalRows - 1) / (targetRows - 1);
        var colScale = (double)(originalCols - 1) / (targetCols - 1);

        // Loop through each element of the new array
        for (var i = 0; i < targetRows; i++)
        {
            for (var j = 0; j < targetCols; j++)
            {
                // Find corresponding element in the original array using scaling
                var originalI = (int)(i * rowScale);
                var originalJ = (int)(j * colScale);

                // Set the value in the new array
                newArray[i, j] = original[originalI, originalJ];
            }
        }

        return newArray;
    }
}