namespace LittleAGames.PFWolf.SDK.Components;

public class MenuItem : Text
{
    private MenuItem(string text, int x, int y, int activeState, Action action)
        : base(text, x, y, "LargeFont", 0x13)
    {
    }
    
    public static MenuItem Create(string text, int x, int y, int activeState, Action action)
        => new MenuItem(text, x, y, activeState, action);
}