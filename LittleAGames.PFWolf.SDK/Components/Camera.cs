namespace LittleAGames.PFWolf.SDK.Components;

public class Camera
{
    private Camera(int x, int y)
    {
        
    }
    
    public static Camera Create(int x, int y)
        => new Camera(x, y);
}