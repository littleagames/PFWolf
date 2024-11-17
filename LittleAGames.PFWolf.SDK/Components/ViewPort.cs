namespace LittleAGames.PFWolf.SDK.Components;

public class ViewPort : RenderComponent
{
    private readonly Camera _camera;
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }

    private ViewPort(int x, int y, int width, int height, Camera camera)
    {
        _camera = camera;
        X = x;
        Y = y;
        Width = width;
        Height = height;
        
        Children.Add(Rectangle.Create(x,y,width,height, 0x01)); // canvas
        
        // Borders
        Children.Add(Rectangle.Create(x-1,y-1,width+1, 1, 0)); // top
        Children.Add(Rectangle.Create(x-1,y+height, width+2, 1, 0x7d)); // bottom
        Children.Add(Rectangle.Create(x-1,y-1,1, height+1, 0)); // left
        Children.Add(Rectangle.Create(x+width,y-1,1, height+1, 0x7d)); // right
        Children.Add(Rectangle.Create(x-1,y+height,1, 1, 0x7c)); // lower left highlight
        
        // TODO: Renderer.Create
        // Renderer will have a 2d array of size width/height
        // It will handle FOV, and whatever to draw within those bounds
        // Should I have several renderers?
            // WallRenderer
            // SpriteRenderer
            // How does it do priority?
            // Do I draw sprites behind walls? (I think it should just get all visible things in priority order, add them all front to back? or back to front?
                // TODO: Then improve later
                // You could toggle walls, sprites, etc (with debug features?)
    }
    
    public static ViewPort Create(int x, int y, int width, int height, Camera camera)
    => new ViewPort(x, y, width, height, camera);
}