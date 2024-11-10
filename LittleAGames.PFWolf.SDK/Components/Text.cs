namespace LittleAGames.PFWolf.SDK.Components;

public class Text : RenderComponent
{
    public string String { get; }
    public int X { get; }
    public int Y { get; }
    public string FontAssetName { get; }
    public byte Color { get; }

    private Text(string text, int x, int y, string fontAssetName, byte color)
    {
        String = text;
        X = x;
        Y = y;
        FontAssetName = fontAssetName;
        Color = color;
    }

    public static Text Create(string text, int x, int y, string fontAssetName, byte color)
        => new(text, x, y, fontAssetName, color);
}