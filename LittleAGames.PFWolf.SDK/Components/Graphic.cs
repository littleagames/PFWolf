namespace LittleAGames.PFWolf.SDK.Components;

public class Graphic : RenderComponent
{
    public string AssetName { get; }
    public int X { get; }
    public int Y { get; }

    private Graphic(string assetName, int x, int y)
    {
        AssetName = assetName;
        X = x;
        Y = y;
    }

    public static Graphic Create(string assetName, int x, int y)
    {
        return new Graphic(assetName, x, y);
    }
}