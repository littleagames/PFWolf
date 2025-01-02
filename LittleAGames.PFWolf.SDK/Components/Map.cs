namespace LittleAGames.PFWolf.SDK.Components;

public class Map : MapComponent
{
    public string AssetName { get; }
    public ushort[][,] Plane { get; set; } = new ushort[3][,];
    
    public Dictionary<int, Wall> Walls { get; set; } = new Dictionary<int, Wall>();

    private Map(string assetName)
    {
        AssetName = assetName;
    }
    
    public static Map Create(string assetName)
        => new(assetName);
    
    public new int Width { get; set; }
    
    public new int Height { get; set; }
    
    /// <summary>
    /// Name given to the map for the game (e.g. "The Castle")
    /// </summary>
    public string Name { get; set; }

    [Obsolete("This is extremely inefficient. Find a better solution")]
    public Wall? FindWall(int tileX, int tileY)
    {
        return Children.GetComponents().OfType<Wall>().FirstOrDefault(x => x.X == tileX && x.Y == tileY);
    }

    public override void OnUpdate()
    {
        // TODO: Map handling stuff?
        
        // Actors?
        // Pushwalls?
        
    }
}

public class Wall : MapComponent
{
    public byte[] North { get; init; }
    public byte[] South { get; init; }
    public byte[] East { get; init; }
    public byte[] West { get; init; }
}

public class Sprite : MapComponent
{
    public byte[] Data { get; init; }
}