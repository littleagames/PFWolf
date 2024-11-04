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

public class FontCharacter
{
    public short Height { get; set; }
    public byte Width { get; set; }
    public byte[] RawData { get; set; } = null!;
}

public sealed class FontAsset : VgaAsset
{
    public FontAsset(string name, byte[] rawData)
    {
        Name = name;
        RawData = rawData;
        AssetType = AssetType.Font;
        FontCharacters = GetFontCharacters();
    }

    public List<FontCharacter> FontCharacters { get; set; }

    private List<FontCharacter> GetFontCharacters()
    {
        var fontChars = new List<FontCharacter>();
        var location = GetLocations();
        var widths = GetWidths();
        var height = GetHeight();
        for (var ascii = 0; ascii < 256; ascii++)
        {
            byte[] fontData = RawData.Skip(location[ascii]).Take(sizeof(byte) * widths[ascii] * height)
                .ToArray();
            
            fontChars.Add(new FontCharacter
            {
                Height = height,
                Width = widths[ascii],
                RawData = fontData,
            });
        }

        return fontChars;
    }
    
    private short GetHeight()
    {
        var height = BitConverter.ToInt16(RawData.Take(2).ToArray());
        if (height > 255)
        {
            throw new InvalidDataException("Font height is too large.");
        }
        
        return height;
    }
    
    private short[] GetLocations()
    {
        short[] location = new short[256];
        Buffer.BlockCopy(RawData, sizeof(short), location, 0, sizeof(short) * 256);
        if (location.ToList().Any(l => l > RawData.Length))
        {
            throw new InvalidDataException("Invalid font data while reading the location data.");
        }

        return location;
    }
    
    private byte[] GetWidths()
    {
        byte[] widths = new byte[256];
        var offset = sizeof(short) + (sizeof(short)*256);
        Buffer.BlockCopy(RawData, offset, widths, 0, sizeof(byte) * 256);
        
        if (widths.ToList().Any(l => l > RawData.Length))
        {
            throw new InvalidDataException("Invalid font data while reading the widths data.");
        }
        
        return widths;
    }
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
    public string Text { get; set; } = null!;
    
    public TextAsset()
    {
        AssetType = AssetType.Text;
    }
}