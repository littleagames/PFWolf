namespace LittleAGames.PFWolf.SDK.Assets;


public class WallAsset : Asset
{
    public WallAsset()
    {
        AssetType = AssetType.Texture;
    }
    
    public override string Name { get; set; }
    public override byte[] RawData { get; set; }
}

public class SpriteAsset : Asset
{
    public SpriteAsset()
    {
        AssetType = AssetType.Sprite;
    }

    public override string Name { get; set; }
    public override byte[] RawData { get; set; }
    public byte[,] Pixels { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Position Offset { get; set; } = new(0, 0);
}

public class DigitizedSoundAsset : Asset
{
    public DigitizedSoundAsset()
    {
        AssetType = AssetType.DigiSound;
    }
    
    private readonly char[] WavHeader =
    [
        'R', 'I', 'F', 'F', (char)0, (char)0, (char)0, (char)0, 'W', 'A', 'V', 'E',
        'f', 'm', 't', ' ', (char)16, (char)0, (char)0, (char)0, (char)1, (char)0, (char)1, (char)0,
        (char)0x82, (char)0x17, (char)0, (char)0, (char)0x37, (char)0x04, (char)0, (char)0, (char)2, (char)0, (char)16, (char)0,
        'd', 'a', 't', 'a', (char)0, (char)0, (char)0, (char)0
    ];

    public override string Name { get; set; }
    public override byte[] RawData { get; set; }
}