namespace LittleAGames.PFWolf.SDK.Components;

public class Map : Component
{
    public string AssetName { get; }
    public int Width { get; set; }
    public int Height { get; set; }

    private Map(string assetName)
    {
        AssetName = assetName;
    }
    public static Map Create(string mapAssetName)
        => new(mapAssetName);
}