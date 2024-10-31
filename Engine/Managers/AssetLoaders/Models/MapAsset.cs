namespace Engine.Managers.AssetLoaders.Models;

public class MapAsset : Asset
{
    public override string Name { get; set; }
    public override byte[] RawData { get; set; }
}