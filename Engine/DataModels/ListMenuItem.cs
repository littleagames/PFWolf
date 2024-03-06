using Engine.Managers;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

namespace Engine.DataModels;

public abstract class ListMenuItem
{
    public abstract void Draw(VideoLayerManager manager);
}

public abstract class ListMenuItemSelectable : ListMenuItem
{
    // TODO: Selectable functionality?
}

public class ListMenuItemTextItem : ListMenuItemSelectable
{
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public FontColor TextColor { get; set; }
    public FontName FontName { get; set; }

    private readonly string _text;
    private readonly string _key;
    private readonly string _menuSwitcherMethodName;

    public ListMenuItemTextItem(string text, string key, string menuSwitcherMethodName)
    {
        _text = text;
        _key = key;
        _menuSwitcherMethodName = menuSwitcherMethodName;
    }

    public override void Draw(VideoLayerManager vl)
    {
        vl.DrawTextString(PositionX, PositionY, _text, FontName, TextColor);
    }
}

public class ListMenuItemPatchItem : ListMenuItemSelectable
{

    public override void Draw(VideoLayerManager manager)
    {
        //throw new NotImplementedException();
    }
}

public class ListMenuItemStaticPatch : ListMenuItem
{
    private readonly string _picName;
    private readonly int _xPosition;
    private readonly int _yPosition;

    public ListMenuItemStaticPatch(int x, int y, string picName)
    {
        _xPosition = x;
        _yPosition = y;
        _picName = picName;
    }

    public override void Draw(VideoLayerManager vl)
    {
        vl.DrawPic(_xPosition, _yPosition, _picName);
    }
}

public class ListMenuItemWindow : ListMenuItem
{
    private readonly int PositionX;
    private readonly int PositionY;
    private readonly int Width;
    private readonly int Height;
    private readonly int Color;
    private readonly int Color1;
    private readonly int Color2;

    public ListMenuItemWindow(int x, int y, int w, int h, int c, int c1, int c2)
    {
        PositionX = x;
        PositionY = y;
        Width = w;
        Height = h;
        Color = c;
        Color1 = c1;
        Color2 = c2;
    }

    public override void Draw(VideoLayerManager vl)
    {
        vl.DrawRectangle(PositionX, PositionY, Width, Height, (byte)Color); // background
        vl.DrawRectangle(PositionX, PositionY, Width, 1, (byte)Color2); // top
        vl.DrawRectangle(PositionX, PositionY, 1, Height, (byte)Color2); // left
        vl.DrawRectangle(PositionX, PositionY + Height, Width, 1, (byte)Color1); // bottom
        vl.DrawRectangle(PositionX + Width, PositionY, 1, Height, (byte)Color1); // right
    }
}