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
        // TODO: This isn't quite what I had in mind. Can I check for properties on the top-level of the object first?
    }

    public static ActorDefinitionAsset Merge(IEnumerable<ActorDefinitionAsset> assets)
    {
        throw new NotImplementedException();
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