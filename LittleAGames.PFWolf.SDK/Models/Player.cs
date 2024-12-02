using LittleAGames.PFWolf.SDK.Components;

namespace LittleAGames.PFWolf.SDK.Models;

public class Player : Actor
{
    public Player(int x, int y, float angle) : base(x, y, angle)
    {
    }

    public Player(Position position, float angle) : base(position, angle)
    {
    }
}