using LittleAGames.PFWolf.SDK.Components;

namespace LittleAGames.PFWolf.SDK.Assets;

public class ScriptAsset : Asset// where T : RunnableBase
{
    public override string Name { get; set; } = string.Empty;
    public override byte[] RawData { get; set; } = [];
    public Type Script { get; init; } = null!;
}

public class SceneAsset : ScriptAsset
{
}