namespace LittleAGames.PFWolf.SDK.Components;

public class Camera : Component
{
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public int PositionZ { get; set; }

    private Camera(int positionX, int positionY, int positionZ)
    {
        PositionX = positionX;
        PositionY = positionY;
        PositionZ = positionZ;
    }
    
    public static Camera Create(int positionX, int positionY, int positionZ)
        => new(positionX, positionY, positionZ);
}