namespace LittleAGames.PFWolf.SDK.Components;

public class Map : MapComponent
{
    public string AssetName { get; }
    
    /// <summary>
    /// The raw IDs found in the game maps file
    /// </summary>
    public ushort[][,] PlaneIds { get; set; } = new ushort[3][,];

    /// <summary>
    /// Contains all complex tile data such as walls, doors, floor codes
    /// </summary>
    public MapComponent[,] TilePlane { get; set; }
    
    /// <summary>
    /// Contains all complex tile data, such as statics, actors
    /// </summary>
    public MapComponent[,] ObjectPlane { get; set; }

    public IList<Actor> Actors { get; set; } = new List<Actor>();
    
    /// <summary>
    /// All loaded wall textures in the map
    /// </summary>
    public Dictionary<int, WallData> TileCache { get; set; } = new();
    
    /// <summary>
    /// All doors in the map
    /// </summary>
    public Dictionary<int, WallData> DoorCache { get; set; } = new();
    
    /// <summary>
    /// All doors in the map
    /// </summary>
    public Dictionary<string, SpriteData> SpriteCache { get; set; } = new();

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

    public override void OnUpdate()
    {
        foreach (var actor in Actors)
        {
            actor.Think();
            // TODO: Handle map things here, like actor behaviors   
        }
        // TODO: Map handling stuff?
        
        // Actors?
        // Pushwalls?
        
    }
}

public class Wall : MapComponent
{
    public int TileId { get; set; }
    public string North { get; init; }
    public string South { get; init; }
    public string East { get; init; }
    public string West { get; init; }
}
// public class Door : MapComponent
// {
//     public int TileId { get; set; }
//     public string North { get; init; }
//     public string South { get; init; }
//     public string East { get; init; }
//     public string West { get; init; }
// }

public class WallData
{
    public byte[] North { get; init; }
    public byte[] South { get; init; }
    public byte[] East { get; init; }
    public byte[] West { get; init; }
}

public class SpriteData : MapComponent
{
    public Position Offset { get; init; } = new(0, 0);
}