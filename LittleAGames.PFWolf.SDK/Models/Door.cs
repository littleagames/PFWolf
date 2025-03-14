﻿using LittleAGames.PFWolf.SDK.Components;

namespace LittleAGames.PFWolf.SDK.Models;

public class Door : MapComponent// Thinker// Wall //: Thinker (to take tilex,y, and maybe have an Update() method
{
    public int TileId { get; set; }
    public string North { get; init; }
    public string South { get; init; }
    public string East { get; init; }
    public string West { get; init; }
    
    public byte Lock { get; set; }
    public DoorAction Action { get; set; } = DoorAction.Closed;
    public short TicCount { get; set; } = 0;
    public short Position { get; set; } = 0;
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