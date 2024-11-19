namespace LittleAGames.PFWolf.SDK.Components;

public class Map : MapComponent
{
    public string AssetName { get; }

    private Map(string assetName)
    {
        AssetName = assetName;
    }
    
    public static Map Create(string assetName)
        => new(assetName);
    
    
    public Wall[,] Walls { get; set; }
    
    public int Width { get; set; }
    
    public int Height { get; set; }
    
    /// <summary>
    /// Name given to the map for the game (e.g. "The Castle")
    /// </summary>
    public string Name { get; set; }
}

public class Wall
{
    public string North { get; init; }
    public string South { get; init; }
    public string East { get; init; }
    public string West { get; init; }
}