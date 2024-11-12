namespace LittleAGames.PFWolf.SDK.Components;

public class MenuItem : Text
{
    private MenuItem(string text, int x, int y, int menuIndex, ActiveState activeState, Action? action)
        : base(text, x, y, "LargeFont", GetMenuColor(activeState, false))
    {
        ActiveState = activeState;
        Action = action;
        MenuIndex = menuIndex;
    }

    public ActiveState ActiveState { get; set; }
    public Action? Action { get; }

    public int MenuIndex { get; private set; }
    public bool IsSelected { get; private set; }
    
    public static MenuItem Create(string text, int x, int y, int menuIndex, ActiveState activeState, Action? action)
        => new MenuItem(text, x, y, menuIndex, activeState, action);

    public void SetIsSelected(bool isSelected)
        => IsSelected = isSelected;
    
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