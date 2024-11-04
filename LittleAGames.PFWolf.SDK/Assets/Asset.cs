namespace LittleAGames.PFWolf.SDK.Assets;

public abstract class Asset
{
    public const string AssetMarker = "!ID!";
        
    public abstract string Name { get; set; }
    public abstract byte[] RawData { get; set; }
    public AssetType AssetType { get; set; } = AssetType.Unknown;

    public override string ToString()
    {
        return $"{Name} [{AssetType}] [{RawData?.Length ?? 0} bytes]";
    }
}