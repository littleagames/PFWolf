namespace LittleAGames.PFWolf.SDK.Assets;

public class MapAsset : Asset
{
    public override string Name { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int NumPlanes { get; set; }
    public override byte[] RawData { get; set; }
}