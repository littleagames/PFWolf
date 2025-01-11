namespace LittleAGames.PFWolf.SDK.Assets;

public class MapDefinitions : Asset
{
    public Dictionary<int, WallDefinition> Walls { get; set; } = new();
    public Dictionary<int, DoorDefinition> Doors { get; set; } = new();

    public TileDefinition? FindWall(int tileId)
    {
        if (Walls.TryGetValue(tileId, out var wallDefinition))
            return wallDefinition;

        return null;
    }
    public TileDefinition? FindDoor(int tileId)
    {
        if (Doors.TryGetValue(tileId, out var doorDefinition))
            return doorDefinition;

        return null;
    }

    public sealed override string Name { get; set; }
    public sealed override byte[] RawData { get; set; }

    public MapDefinitions(string name, byte[] rawData)
    {
        AssetType = AssetType.MapDefinitions;
        Name = name;
        RawData = rawData;
        // TODO: json or yaml (pass the formatter/parser in as a parameter
        var encoded = System.Text.Encoding.UTF8.GetString(rawData);
        var defs = System.Text.Json.JsonSerializer.Deserialize<MapDefinitionFile>(encoded);
        if (defs == null)
        {
            // throw error
        }
        Walls = defs.Walls.ToDictionary(x => Convert.ToInt32(x.Key), x => x.Value); 
        Doors = defs.Doors.ToDictionary(x => Convert.ToInt32(x.Key), x => x.Value);
        // TODO: This isn't quite what I had in mind. Can I check for properties on the top-level of the object first?
    }

    public void Merge(byte[] rawData /*,formatloader*/)
    {
        var encoded = System.Text.Encoding.UTF8.GetString(rawData);
        var defs = System.Text.Json.JsonSerializer.Deserialize<MapDefinitionFile>(encoded);
        var walls = defs.Walls.ToDictionary(x => Convert.ToInt32(x.Key), x => x.Value);
        var doors = defs.Doors.ToDictionary(x => Convert.ToInt32(x.Key), x => x.Value);
        
        Walls = Walls.Concat(walls)
            .GroupBy(kv => kv.Key)
            .ToDictionary(g => g.Key, g => g.Last().Value);
        
        Doors = Doors.Concat(doors)
            .GroupBy(kv => kv.Key)
            .ToDictionary(g => g.Key, g => g.Last().Value);
        // TODO: Add new, overwrite existing? (with warning?) It came from the same pk3 file
    }
}

public class MapDefinitionFile
{
    public Dictionary<string, WallDefinition> Walls { get; set; } = new();
    public Dictionary<string, DoorDefinition> Doors { get; set; } = new();   
}

public class TileDefinition
{
    public string North { get; set; } = null!;
    public string South { get; set; } = null!;
    public string West { get; set; } = null!;
    public string East { get; set; } = null!;
}

public class WallDefinition : TileDefinition
{
    
}

public class DoorDefinition : TileDefinition
{

}