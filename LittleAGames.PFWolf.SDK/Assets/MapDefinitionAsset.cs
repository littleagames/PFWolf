namespace LittleAGames.PFWolf.SDK.Assets;

public class MapDefinitionAsset : Asset
{
    public Dictionary<int, WallDefinition> Walls { get; init; }
    public Dictionary<int, DoorDefinition> Doors { get; init; }
    public Dictionary<int, ActorDefinition> Actors { get; init; }
    public Dictionary<int, PlayerDefinition> Player { get; init; }

    public TileDefinition? FindWall(int tileId)
    {
        return Walls.GetValueOrDefault(tileId);
    }
    public TileDefinition? FindDoor(int tileId)
    {
        return Doors.GetValueOrDefault(tileId);
    }

    public sealed override string Name { get; set; }
    public sealed override byte[] RawData { get; set; }
    public List<string> MapDefinitions { get; init; }

    private MapDefinitionAsset(string name)
    {
        AssetType = AssetType.MapDefinitions;
        Name = name;
        RawData = [];
    }
    public MapDefinitionAsset(string name, byte[] rawData)
    {
        AssetType = AssetType.MapDefinitions;
        Name = name;
        RawData = rawData;
        // TODO: json or yaml (pass the formatter/parser in as a parameter
        var encoded = System.Text.Encoding.UTF8.GetString(rawData);
        var defs = System.Text.Json.JsonSerializer.Deserialize<MapDefinitionDataModel>(encoded);
        if (defs == null)
        {
            // throw error
        }
        Walls = defs.Walls.ToDictionary(x => Convert.ToInt32(x.Key), x => x.Value); 
        Doors = defs.Doors.ToDictionary(x => Convert.ToInt32(x.Key), x => x.Value);
        Actors = defs.Actors.ToDictionary(x => Convert.ToInt32(x.Key), x => x.Value);
        Player = defs.Player.ToDictionary(x => Convert.ToInt32(x.Key), x => x.Value);
        MapDefinitions = defs.MapDefinitions;
        // TODO: This isn't quite what I had in mind. Can I check for properties on the top-level of the object first?
    }

    public static MapDefinitionAsset Merge(IEnumerable<MapDefinitionAsset> assets)
    {
        var walls = new Dictionary<int, WallDefinition>();
        var doors = new Dictionary<int, DoorDefinition>();
        var actors = new Dictionary<int, ActorDefinition>();
        var player = new Dictionary<int, PlayerDefinition>();

        foreach (var asset in assets)
        {
            walls = walls.Concat(asset.Walls)
                .GroupBy(kv => kv.Key)
                .ToDictionary(g => g.Key, g => g.Last().Value);

            doors = doors.Concat(asset.Doors)
                .GroupBy(kv => kv.Key)
                .ToDictionary(g => g.Key, g => g.Last().Value);
            // TODO: Add new, overwrite existing? (with warning?) It came from the same pk3 file

            actors = actors.Concat(asset.Actors)
                .GroupBy(kv => kv.Key)
                .ToDictionary(g => g.Key, g => g.Last().Value);

            player = player.Concat(asset.Player)
                .GroupBy(kv => kv.Key)
                .ToDictionary(g => g.Key, g => g.Last().Value);
        }

        return new MapDefinitionAsset("map-definitions")
        {
            Walls = walls,
            Doors = doors,
            Actors = actors,
            Player = player
        };
    }
}

public record MapDefinitionDataModel
{
    public Dictionary<string, WallDefinition> Walls { get; set; } = new();
    public Dictionary<string, DoorDefinition> Doors { get; set; } = new(); 
    public Dictionary<string, ActorDefinition> Actors { get; set; } = new();
    public Dictionary<string, PlayerDefinition> Player { get; set; } = new();
    public List<string> MapDefinitions { get; set; } = [];
}

public record TileDefinition
{
    public string North { get; set; } = null!;
    public string South { get; set; } = null!;
    public string West { get; set; } = null!;
    public string East { get; set; } = null!;
}

public record WallDefinition : TileDefinition
{
    
}

public record DoorDefinition : TileDefinition
{

}

public record ActorDefinition
{
    public List<ActorStateDefinition> Spawn { get; set; } = [];
    public List<ActorStateDefinition> Walk { get; set; } = [];
}

public record PlayerDefinition
{
    public string Actor { get; set; }
    public ActorDataDefinition Params { get; set; }
}

public record ActorDataDefinition
{
    public object Direction { get; set; }
    public object Angle { get; set; }
    public object Health { get; set; }
    public List<object> Ammo { get; set; }
    public List<object> Weapons { get; set; }
}


public class ActorStateDefinition
{
    /// <summary>
    /// Graphic assigned to the frame
    /// </summary>
    public string Frame { get; set; }

    /// <summary>
    /// Number of tics to display frame, -1 means it runs indefinitely
    /// </summary>
    public float Tics { get; set; } = -1;

    /// <summary>
    /// String pipe delimited list of default flags on the actor
    /// </summary>
    public string Flags { get; set; } = null!;
}

public enum ActorFlags
{
    None = 0,
    Block = 1,
}