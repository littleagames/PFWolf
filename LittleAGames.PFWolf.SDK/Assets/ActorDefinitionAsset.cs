using System.Text.Json.Serialization;

namespace LittleAGames.PFWolf.SDK.Assets;

public class ActorDefinitionAsset : Asset
{
    private ActorDefinitionAsset(string name)
    {
        AssetType = AssetType.ActorDefinitions;
        Name = name;
        RawData = [];
    }
    public ActorDefinitionAsset(string name, byte[] rawData)
    {
        AssetType = AssetType.ActorDefinitions;
        Name = name;
        RawData = rawData;
        // TODO: json or yaml (pass the formatter/parser in as a parameter
        var encoded = System.Text.Encoding.UTF8.GetString(rawData);
        var defs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, ActorDefinitionDataModel>>(encoded);
        if (defs == null)
        {
            // throw error
        }

        Actors = defs;
    }

    // TODO: ActorDefinitionDataModel and ActorMapDefinition need to be merged or figured out
    // I don't remember what I did here, but I think I need to have both, and build them together
    public Dictionary<string, ActorDefinitionDataModel> Actors { get; set; } = null!;

    public static ActorDefinitionAsset Merge(IEnumerable<ActorDefinitionAsset> assets)
    {
        var actors = new Dictionary<string, ActorDefinitionDataModel>();

        foreach (var asset in assets)
        {
            actors = actors.Concat(asset.Actors)
                .GroupBy(kv => kv.Key)
                .ToDictionary(g => g.Key, g => g.Last().Value);
        }
        
        return new ActorDefinitionAsset("actor-definitions")
        {
            Actors = actors
        };
    }

    public override string Name { get; set; }
    public override byte[] RawData { get; set; }
}

public record ActorDefinitionDataModel
{
    public Dictionary<string, List<ActorStateDefinitionDataModel>> States { get; set; } = new();
    public int HitPoints { get; set; }
}

public class ActorStateDefinitionDataModel
{
    public bool Directional { get; set; }
    public string Frame { get; set; }
    public int Tics { get; set; }
    public string Think { get; set; }
    public string Action { get; set; }
}