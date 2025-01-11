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
    
    /// <summary>
    /// All loaded wall textures in the map
    /// </summary>
    public Dictionary<int, Wall> WallCache { get; set; } = new();
    
    /// <summary>
    /// All doors in the map
    /// </summary>
    public Dictionary<int, Wall> DoorCache { get; set; } = new();

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