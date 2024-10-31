namespace Engine.Managers.AssetLoaders.Models;

public abstract class Asset
{
    public abstract string Name { get; set; }
    public abstract byte[] RawData { get; set; }
    public AssetType AssetType { get; set; } = AssetType.Unknown;
}