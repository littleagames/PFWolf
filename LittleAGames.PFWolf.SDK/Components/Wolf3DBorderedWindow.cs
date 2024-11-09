namespace LittleAGames.PFWolf.SDK.Components;

public class Wolf3DBorderedWindow : GroupedRenderComponent
{
    
    public static Wolf3DBorderedWindow Create(int x, int y, int width, int height)
        => new(x, y, width, height);
    
    private Wolf3DBorderedWindow(int x, int y, int width, int height)
    {
        Components.Add(Rectangle.Create(x,y,width,height, 0x2d));
        
        // Borders
        Components.Add(Rectangle.Create(x,y,width, 1, 0x2b)); // top
        Components.Add(Rectangle.Create(x,y+height, width, 1, 0x23)); // bottom
        Components.Add(Rectangle.Create(x,y,1, height, 0x2b)); // left
        Components.Add(Rectangle.Create(x+width,y,1, height, 0x23)); // right
    }
}
