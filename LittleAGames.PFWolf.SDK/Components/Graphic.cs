namespace LittleAGames.PFWolf.SDK.Components;

public class Graphic : RenderComponent
{
    public string AssetName { get; }
    public int X { get; private set; }
    public int Y { get; private set; }

    private Graphic(string assetName, int x, int y)
    {
        AssetName = assetName;
        X = x;
        Y = y;
    }

    public static Graphic Create(string assetName, int x, int y)
        => new Graphic(assetName, x, y);

    public static Graphic Create(string assetName, Position position)
        =>  new Graphic(assetName, position.X, position.Y);
    
    public void SetPosition(Position position)
    {
        X = position.X;
        Y = position.Y;
    }
}