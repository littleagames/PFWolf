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
    public List<MenuItem> MenuItems { get { return MenuComponents.Where(mc => mc is MenuItem).Select(mc => (MenuItem)mc).ToList(); } }

    private int _cursorPosition = -1;
    private string _cursorGraphic;
    private int _offsetX = 0;
    private int _offsetY = 0;

    public void SetCursor(string cursorPic, int offsetX, int offsetY)
    {
        _cursorGraphic = cursorPic;
        _offsetX = offsetX;
        _offsetY = offsetY;
    }

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

        var firstItem = MenuItems.FirstOrDefault(); // this skips the menuswitcheritems
        // cursor
        if (firstItem != null)
        {
            vl.DrawPic(
                firstItem.PositionX + _offsetX,
                firstItem.PositionY + _offsetY,
                _cursorGraphic);
        }

        vl.UpdateScreen();
    }

    public int Handle()
    {
        //var exit = 0;

        //do
        //{
        //    if (_indent > 0) // and lastblinktime
        //    {
        //        // TODO: This is the cursor animation
        //        _cursorGraphic = "menus/cursor1";
        //        Draw();
        //    }

        //    // if (key == enter) exit = 1;
        //    // else if (key == esc exit = 2;

        //} while (exit == 0);
        return -1;
    }
}

internal class MenuItem : MenuComponent
{
    internal MenuItem(
        int positionX,
        int positionY,
        string text,
        FontColor textColor,
        FontColor disabledTextColor,
        FontColor highlightedTextColor,
        Func<int, bool>? listener = null)
        : base(positionX, positionY)
    {
        Text = text;
        TextColor = textColor;
        DisabledTextColor = disabledTextColor;
        HighlightedTextColor = highlightedTextColor;
        Listener = listener;
    }

    public string Text { get; } = string.Empty;
    public FontColor TextColor { get; }
    public FontColor DisabledTextColor { get; }
    public FontColor HighlightedTextColor { get; }

    /// <summary>
    /// Menu item is able to be selected, and the action (if applicable) can be performed
    /// </summary>
    public bool Enabled { get; set; } = false;

    /// <summary>
    /// The menu item is currently selected
    /// </summary>
    public bool IsHighlighted { get; set; } = false;

    /// <summary>
    /// The menu item will not be visible or counted in the displayed list
    /// </summary>
    public bool Visible { get; set; } = false;
    public Func<int, bool>? Listener { get; } = null!;

    public override void Draw()
    {
        var vl = VideoLayerManager.Instance;

        FontColor color;
        if (!Enabled)
            color = DisabledTextColor;
        if (IsHighlighted)
            color = HighlightedTextColor;
        else
            color = TextColor;

        vl.DrawTextString(PositionX, PositionY, Text, new FontName("font/bigfont"), color);
    }
}

internal class MenuSwitcherItem : MenuItem
{
    public MenuSwitcherItem(int positionX, int positionY, string text, FontColor textColor,
        FontColor disabledTextColor,
        FontColor highlightedTextColor,
        Menu menu,
        Func<int, bool>? listener = null)
        :base (positionX, positionY, text, textColor, disabledTextColor, highlightedTextColor, listener)
    {
        Menu = menu;
    }

    public Menu Menu { get; }
}

internal class MenuWindow : MenuComponent
{
    public int Width { get; } = 0;
    public int Height { get; } = 0;
    public FontColor Color { get; }
    public FontColor Color1 { get; }
    public FontColor Color2 { get; }

    public MenuWindow(int positionX, int positionY, int width, int height, FontColor color, FontColor color1, FontColor color2)
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
        vl.DrawRectangle(PositionX, PositionY, Width, 1, Color2.GetByte()); // top
        vl.DrawRectangle(PositionX, PositionY, 1, Height, Color2.GetByte()); // left
        vl.DrawRectangle(PositionX, PositionY + Height, Width, 1, Color1.GetByte()); // bottom
        vl.DrawRectangle(PositionX + Width, PositionY, 1, Height, Color1.GetByte()); // right
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