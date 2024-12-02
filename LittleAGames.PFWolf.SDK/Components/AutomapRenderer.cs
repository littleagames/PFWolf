namespace LittleAGames.PFWolf.SDK.Components;

public class AutoMapRenderer : Renderer
{
    private readonly Map _map;

    private AutoMapRenderer(Camera camera, Map map)
    {
        Camera = camera;
        _map = map;
        Scale = 16;
    }
    
    private AutoMapRenderer(Map map)
    {
        _map = map;
        Scale = 16;
    }

    public static AutoMapRenderer Create(Camera camera, Map map)
        => new(camera, map);
    
    public static AutoMapRenderer Create(Map map)
        => new(map);

    public override byte[] Render(int width, int height)
    {
        var graphic = new byte[width * height];

        var mapHeight = _map.Height;
        var mapWidth = _map.Width;
        
        var scaledMapWidth = (mapWidth * Scale);
        var scaledMapHeight = (mapHeight * Scale);
        
        var centerX = width / 2;
        var centerY = height / 2;
        
        var cameraX = Camera?.Position.X ?? 0;
        var cameraY = Camera?.Position.Y ?? 0;
        
        var scaledCameraX = (cameraX * Scale);
        var scaledCameraY = (cameraY * Scale);

        // Gets the scaled center X/Y point of the map that
        // fit in the centerX, centerY
        var mapCenterX = scaledCameraX + Scale / 2;
        var mapCenterY = scaledCameraY + Scale / 2;
        
        var topBound = mapCenterY - centerY;
        var bottomBound = mapCenterY + centerY;
        var leftBound = mapCenterX - centerX;
        var rightBound = mapCenterX + centerX;
//Sutherland–Hodgman algorithm
        foreach (var component in _map.FindComponents<MapComponent>())
        {
            for (var gfxY = 0; gfxY < component.Height; gfxY++)
            {
                for (var gfxX = 0; gfxX < component.Width; gfxX++)
                {
                    graphic[gfxY * width + gfxX] = component.Data[gfxX, gfxY]; // ScaleImage()
                }
            }

            break;
            // if (component.X < leftBound/Scale) continue;
            // if (component.X > rightBound/Scale) continue;
            // if (component.Y < topBound/Scale) continue;
            // if (component.Y > bottomBound/Scale) continue;
            //
            // var scaleX = component.X*Scale;
            // var scaleY = component.Y*Scale;
            //
            // var x0 = 0;
            // for (var x = leftBound; x < rightBound && x < component.Width * Scale; x++, x0++)
            // {
            //     var y0 = 0;
            //     for (var y = topBound; y < bottomBound && y < component.Height * Scale; y++,y0++)
            //     {
            //         var gfxX = (x - scaleX) / Scale;
            //         var gfxY = (y - scaleY) / Scale;
            //         if (gfxX < 0 || gfxY < 0) continue;
            //         graphic[y0 * width + x0] = component.Data[gfxX, gfxY];
            //     }
            // }
        }
        
        // // TODO: Calculate left bound
        // var topBound = mapCenterY - centerY;
        // var bottomBound = mapCenterY + centerY;
        // var leftBound = mapCenterX - centerX;
        // var rightBound = mapCenterX + centerX;
        //
        // // TODO: foreach (components)
        // // Ignore any outside of the bounds of the viewport
        //
        // foreach (var component in _map.FindComponents<MapComponent>())
        // {
        //     // TODO: Check if component is in bounds
        //     if (component.X < leftBound) continue;
        //     if (component.X > rightBound) continue;
        //     if (component.Y < topBound) continue;
        //     if (component.Y > bottomBound) continue;
        //     
        //     // TODO: Locate origin on viewport for component (X, and Y are the tile locations
        //     //var gfxX =(int)(((leftBound+component.X) - (Scale*originalX)) * 64/Scale); // TODO: Replace 64 with graphic width/height
        //     //var gfxY =(int)(((topBound+component.Y) - (Scale*originalY)) * 64/Scale);
        //     
        // }
        //
        // for (var y = 0; y < height; y++)
        // {
        //     for (var x = 0; x < width; x++)
        //     {   
        //         var originalX = (int) ((leftBound+x) / Scale);
        //         var originalY = (int) ((topBound+y) / Scale);
        //         
        //         // if (originalX < 0 || originalX >= _map.Walls.GetLength(0))
        //         //     continue;
        //         //
        //         // if (originalY < 0 || originalY >= _map.Walls.GetLength(1))
        //         //     continue;
        //         //
        //         // // DrawWalls
        //         // var wall = _map.Walls[originalX, originalY];
        //         // if (wall != null && wall.North != null)
        //         // {
        //         //     var gfxX =(int)(((leftBound+x) - (Scale*originalX)) * 64/Scale); // TODO: Replace 64 with graphic width/height
        //         //     var gfxY =(int)(((topBound+y) - (Scale*originalY)) * 64/Scale);
        //         //     if (gfxX < 0 || gfxX >= 64 || gfxY < 0 || gfxY >= 64)
        //         //         continue;
        //         //     graphic[y * width + x] = wall.North[gfxX * 64 + gfxY]; // TODO: Raw data is rotated for easier vertical line rendering in raycast
        //         // }
        //         
        //         // else
        //         // {
        //         //     graphic[y * width + x] = 0;
        //         // }
        //     }
        // }

        
        // foreach (var child in _map.FindComponents<Sprite>())
        // {
        //     // var arrowGraphic = RotateExtensions.Rotate(child.Data.To2DArray(64,64), Camera?.AttachedActor?.Angle ?? 0);
        //     //
        //     for (int x = 0; x < 64; x++)
        //     {
        //         for (int y = 0; y < 64; y++)
        //         {
        //             graphic[(centerY - 32 + y) * width + (centerX - 32 + x)] =
        //                 child.Data[y * 64 + x]; //arrowGraphic[x,y];
        //         }
        //     }
        // }
        
        return graphic; // This could be a lot larger than this
    }
}