using Engine.Managers;
using System.Drawing;

namespace Engine.DataModels;

internal interface IMenu
{
    void Draw();
}

internal class Menu : IMenu
{
    const byte BORDCOLOR = 0x29;
    const byte BORD2COLOR = 0x23;
    const byte DEACTIVE = 0x2b;
    const byte BKGDCOLOR = 0x2d;
    const byte STRIPE = 0x2c;

    const byte MENU_X = 76;
    const byte MENU_Y = 55;
    const byte MENU_W = 178;
    const byte MENU_H = 13 * 10 + 6;

    public List<MenuItem> MenuItems { get; private set; } = new List<MenuItem>();

    public void AddItem(MenuItem item)
    {
        MenuItems.Add(item);
    }

    public void Draw()
    {
        var vl = VideoLayerManager.Instance;
        vl.DrawBackground(BORDCOLOR);

        // Draw gfx
        vl.DrawPic(112, 184, "menus/mouselback"); // bottom centered (need tools to do that)
        DrawStripes(10);
        vl.DrawPic(84, 0, "menus/options");

        // Draw window
        DrawWindow(MENU_X - 8, MENU_Y - 3, MENU_W, MENU_H, BKGDCOLOR, BORD2COLOR, DEACTIVE);

        
        vl.UpdateScreen();
    }

    public int Handle()
    {
        return -1;
    }

    private void DrawStripes(int y)
    {
        var vl = VideoLayerManager.Instance;
        vl.DrawRectangle(0, y, 320, 24, 0x00);
        vl.DrawRectangle(0, y+22, 320, 1, STRIPE);
    }

    private void DrawWindow(int x, int y, int width, int height, byte color, byte color1, byte color2)
    {
        var vl = VideoLayerManager.Instance;
        vl.DrawRectangle(x, y, width, height, color); // background
        
        vl.DrawRectangle(x, y, width, 1, color2); // top
        vl.DrawRectangle(x, y, 1, height, color2); // left
        vl.DrawRectangle(x, y+height, width, 1, color1); // bottom
        vl.DrawRectangle(x+width, y, 1, height, color1); // right

    }
}

internal abstract class MenuItem
{
    public string Text { get; set; } = string.Empty;
    public bool Enabled { get; set; } = false;
    public bool IsHighlighted { get; set; } = false;
    public bool Visible { get; set; } = false;
    public int PositionX { get; set; } = 0;
    public int PositionY { get; set; } = 0;
    public Func<Menu, int> Listener { get; } = null!;
}

internal class MenuSwitcherItem : MenuItem
{
    public MenuSwitcherItem(string text, Func<Menu, int> listener)
    {
        Text = text;
    }
}
