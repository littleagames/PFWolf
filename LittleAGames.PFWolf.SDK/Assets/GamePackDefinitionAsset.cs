namespace LittleAGames.PFWolf.SDK.Assets;

public class GamePackDefinitionAsset : Asset
{
    public sealed override string Name { get; set; }
    public sealed override byte[] RawData { get; set; }

    /// <summary>
    /// Asset name of the map definitions 
    /// </summary>
    public string MapDefinitionsAssetName { get; set; }

    /// <summary>
    /// Default game palette used for the game pack
    /// </summary>
    public string GamePalette { get; set; }

    /// <summary>
    /// Scene that is first loaded in for the game to start
    /// </summary>
    public string StartingScene { get; set; }

    public GamePackDefinitionAsset(string name, byte[] rawData)
    {
        AssetType = AssetType.GamePackDefinition;
        Name = name;
        RawData = rawData;
        // TODO: json or yaml (pass the formatter/parser in as a parameter
        var encoded = System.Text.Encoding.UTF8.GetString(rawData);
        var defs = System.Text.Json.JsonSerializer.Deserialize<GamePackDefinitionDataModel>(encoded);
        if (defs == null)
        {
            // throw error
        }

        MapDefinitionsAssetName = defs.MapDefinitions;
        GamePalette = defs.GamePalette;
        StartingScene = defs.StartingScene;
    }
}

public record GamePackDefinitionDataModel
{
    /// <summary>
    /// Asset name of the map definitions 
    /// </summary>
    public string MapDefinitions { get; init; } = null!;

    /// <summary>
    /// Default game palette used for the game pack
    /// </summary>
    public string GamePalette { get; init; } = null!;

    /// <summary>
    /// Scene that is first loaded in for the game to start
    /// </summary>
    public string StartingScene { get; init; } = null!;
}