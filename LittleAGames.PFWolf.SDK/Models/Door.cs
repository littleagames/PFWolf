using LittleAGames.PFWolf.SDK.Components;

namespace LittleAGames.PFWolf.SDK.Models;

public class Door : Thinker// Wall //: Thinker (to take tilex,y, and maybe have an Update() method
{
    public bool IsVertical { get; set; }
    public byte Lock { get; set; }
    public DoorAction Action { get; set; }
    public short TicCount { get; set; }
    public short Position { get; set; }
}

public enum DoorAction
{
    Open,
    Closed,
    Opening,
    Closing
}

public class Thinker : Component
{
    
}