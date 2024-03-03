using Engine.Managers;

namespace Engine.DataModels;

internal interface IMenu
{
    void Draw();
}

internal class Menu : IMenu
{
    public FontColor BackgroundColor { get; private set; } = new FontColor(0x00);
    public List<MenuComponent> MenuComponents { get; private set; } = new List<MenuComponent>();

    public void SetBackgroundColor(FontColor backgroundColor)
    {
        BackgroundColor = backgroundColor;
    }

    public void AddItem(MenuComponent component)
    {
        MenuComponents.Add(component);
    }

    public void Draw()
    {
        var vl = VideoLayerManager.Instance;
        vl.DrawBackground(BackgroundColor.GetByte());
        MenuComponents.ForEach(component => component.Draw());

        vl.UpdateScreen();
    }

    public int Handle()
    {
        return -1;
    }
}

internal abstract class MenuItem : MenuComponent
{
    protected MenuItem(int positionX, int positionY, string text, FontColor textColor)
        : base(positionX, positionY)
    {
        Text = text;
        TextColor = textColor;
    }

    public string Text { get; } = string.Empty;
    public FontColor TextColor { get; }
    public bool Enabled { get; set; } = false;
    public bool IsHighlighted { get; set; } = false;
    public bool Visible { get; set; } = false;
    public Func<Menu, int> Listener { get; } = null!;
}

internal class MenuSwitcherItem : MenuItem
{
    public MenuSwitcherItem(int positionX, int positionY, string text, FontColor textColor, Func<Menu, int>? listener = null)
        :base (positionX, positionY, text, textColor)
    {
    }

    public override void Draw()
    {
        var vl = VideoLayerManager.Instance;
        vl.DrawTextString(PositionX, PositionY, Text, new FontName("font/bigfont"), TextColor);
    }
}

internal class MenuWindow : MenuComponent
{
    public int Width { get; } = 0;
    public int Height { get; } = 0;
    public FontColor Color { get; }
    public byte Color1 { get; }
    public byte Color2 { get; }

    public MenuWindow(int positionX, int positionY, int width, int height, FontColor color, byte color1, byte color2)
        : base(positionX, positionY)
    {
        Width = width;
        Height = height;
        Color = color;
        Color1 = color1;
        Color2 = color2;
    }

    public override void Draw()
    {
        var vl = VideoLayerManager.Instance;
        vl.DrawRectangle(PositionX, PositionY, Width, Height, Color.GetByte()); // background
        vl.DrawRectangle(PositionX, PositionY, Width, 1, Color2); // top
        vl.DrawRectangle(PositionX, PositionY, 1, Height, Color2); // left
        vl.DrawRectangle(PositionX, PositionY + Height, Width, 1, Color1); // bottom
        vl.DrawRectangle(PositionX + Width, PositionY, 1, Height, Color1); // right
    }
}

internal class MenuStripe : MenuComponent
{
    public MenuStripe(int positionY, FontColor stripeColor)
        : base(0, positionY)
    {
        StripeColor = stripeColor;
    }

    public FontColor StripeColor { get; }

    public override void Draw()
    {
        var vl = VideoLayerManager.Instance;
        vl.DrawRectangle(PositionX, PositionY, 320, 24, 0x00);
        vl.DrawRectangle(PositionX, PositionY + 22, 320, 1, StripeColor.GetByte());
    }
}

internal class MenuGraphic : MenuComponent
{
    public string Graphic { get; private set; }

    public MenuGraphic(int positionX, int positionY, string graphic)
        : base(positionX, positionY)
    {
        Graphic = graphic;
    }

    public override void Draw()
    {
        var vl = VideoLayerManager.Instance;
        vl.DrawPic(PositionX, PositionY, Graphic);
    }
}

internal abstract class MenuComponent
{
    public int PositionX { get; set; } = 0;
    public int PositionY { get; set; } = 0;

    public MenuComponent(int positionX, int positionY)
    {
        PositionX = positionX;
        PositionY = positionY;
    }

    public abstract void Draw();
}