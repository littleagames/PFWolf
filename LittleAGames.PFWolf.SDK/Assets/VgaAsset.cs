namespace LittleAGames.PFWolf.SDK.Assets;

public abstract class VgaAsset : Asset
{
    public override string Name { get; set; } = null!;
    public override byte[] RawData { get; set; } = [];
}

public class Dimension(ushort width, ushort height)
{
    public ushort Width { get; private set; } = width;
    public ushort Height { get; private set; } = height;
}

public class StructPicAsset : VgaAsset
{
    public StructPicAsset()
    {
        AssetType = AssetType.VgaPicData;
    }
    
    public int NumFonts { get; set; }
    public int NumPics { get; set; }
    public List<Dimension> Dimensions { get; set; } = [];
}

public class FontAsset : VgaAsset
{
    public FontAsset()
    {
        AssetType = AssetType.Font;
    }
    
    public Dimension Dimensions { get; set; } = null!;
}

public class GraphicAsset : VgaAsset
{
    public GraphicAsset()
    {
        AssetType = AssetType.Graphic;
    }
    
    public Dimension Dimensions { get; set; } = null!;
}

public class Tile8Asset : VgaAsset
{
    public Tile8Asset()
    {
        AssetType = AssetType.Tile8;
    }
}

public class EndScreenAsset : VgaAsset
{
    public EndScreenAsset()
    {
        AssetType = AssetType.EndScreen;
    }
}

public class DemoAsset : VgaAsset
{
    public DemoAsset()
    {
        AssetType = AssetType.Demo;
    }

    public int MapNumber { get; set; }
}

public class PaletteAsset : VgaAsset
{
    public PaletteAsset()
    {
        AssetType = AssetType.Palette;
    }
}

public class TextAsset : VgaAsset
{
    public TextAsset()
    {
        AssetType = AssetType.Text;
    }
}