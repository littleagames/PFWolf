namespace LittleAGames.PFWolf.SDK.Components;

public class Menu : RenderComponent
{
    public int X { get; }
    public int Y { get; }
    public int MenuTextIndent { get; }
    public int LineSpacing { get; }

    private IList<MenuItem> MenuItems =>
        Children.GetComponents().Where(c => c.GetType().IsAssignableFrom(typeof(MenuItem))).Select(x => (MenuItem)x).ToList();
    
    private Menu(int x, int y, int menuTextIndent, int lineSpacing)
    {
        X = x;
        Y = y;
        MenuTextIndent = menuTextIndent;
        LineSpacing = lineSpacing;
    }

    public static Menu Create(int x, int y, int menuTextIndent, int lineSpacing)
        => new Menu(x, y, menuTextIndent, lineSpacing);

    public void AddMenuItem(string text, int activeState, Action action)
    {
        var itemX = X + MenuTextIndent;
        var itemY = Y + MenuItems.Count() * LineSpacing;
        Children.Add(MenuItem.Create(text,  itemX, itemY, activeState, action));
    }
}