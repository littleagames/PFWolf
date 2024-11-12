namespace LittleAGames.PFWolf.SDK.Components;

public class Menu : RenderComponent
{
    public int X { get; }
    public int Y { get; }
    public int MenuTextIndent { get; }
    public int LineSpacing { get; }

    public int CurrentIndex { get; private set; }
    
    private IList<MenuItem> MenuItems =>
        Children.GetComponents().Where(c => c.GetType().IsAssignableFrom(typeof(MenuItem))).Select(x => (MenuItem)x).ToList();

    private Menu(int x, int y, int menuTextIndent, int lineSpacing)
    {
        X = x;
        Y = y;
        MenuTextIndent = menuTextIndent;
        LineSpacing = lineSpacing;
    }

    public static Menu Create()
        => new(0, 0, 0, 0);
    
    public static Menu Create(int x, int y, int menuTextIndent, int lineSpacing)
        => new Menu(x, y, menuTextIndent, lineSpacing);

    public void AddMenuItem(string text, ActiveState activeState, Action action)
    {
        var menuIndex = MenuItems.Count;
        var itemX = X + MenuTextIndent;
        var itemY = Y + menuIndex * LineSpacing;
        Children.Add(MenuItem.Create(text, itemX, itemY, menuIndex, activeState, action));
    }

    public void MoveUp()
    {
        var next = CurrentIndex;
        do
        {
            if (next <= 0)
                next = MenuItems.Count - 1;
            else
                next--;

        } while (MenuItems[next].ActiveState == ActiveState.Disabled);

        CurrentIndex = next;
    }

    public void MoveDown()
    {
        var next = CurrentIndex;
        do
        {
            if (next >= MenuItems.Count - 1)
                next = 0;
            else
                next++;

        } while (MenuItems[next].ActiveState == ActiveState.Disabled);

        CurrentIndex = next;
    }

    public void PerformAction()
    {
        MenuItems[CurrentIndex].Action?.Invoke();
    }
    
    public override void OnUpdate()
    {
        foreach (var component in Children.GetComponents())
        {
            var child = (MenuItem)component;
            var isSelected = CurrentIndex == child.MenuIndex;
            child.SetColor(GetMenuColor(child.ActiveState,isSelected));
        }
    }
    private static byte GetMenuColor(ActiveState activeState, bool isSelected)
    {
        if (isSelected)
        {
            return activeState switch
            {  
                // highlighted
                ActiveState.Disabled => 0x2b,
                ActiveState.Active => 0x13,
                ActiveState.Special => 0x47,
                //3 => 0x67,
                _ => 0x13
            };
        }
        
        return activeState switch
        {  
            // normal
            ActiveState.Disabled => 0x2b,
            ActiveState.Active => 0x17,
            ActiveState.Special => 0x4a,
            //3 => 0x6b,
            _ => 0x17
        };
    }
}

public enum ActiveState
{
    Disabled,
    Active,
    Special
}