namespace LittleAGames.PFWolf.SDK.Assets;

public class MapAsset : Asset
{
    public override string Name { get; set; } = null!;
    public int Width { get; init; }
    public int Height { get; init; }
    public int NumPlanes { get; init; }
    public override byte[] RawData { get; set; } = [];
    public ushort[][] PlaneData { get; init; } = [];
}