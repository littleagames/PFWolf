namespace LittleAGames.PFWolf.SDK.Components;

public class Wolf3DBorderedWindow : RenderComponent
{
    
    public static Wolf3DBorderedWindow Create(int x, int y, int width, int height)
        => new(x, y, width, height);
    
    private Wolf3DBorderedWindow(int x, int y, int width, int height)
    {
        Children.Add(Rectangle.Create(x,y,width,height, 0x2d));
        
        // Borders
        Children.Add(Rectangle.Create(x,y,width, 1, 0x2b)); // top
        Children.Add(Rectangle.Create(x,y+height, width, 1, 0x23)); // bottom
        Children.Add(Rectangle.Create(x,y,1, height, 0x2b)); // left
        Children.Add(Rectangle.Create(x+width,y,1, height, 0x23)); // right
    }
}
