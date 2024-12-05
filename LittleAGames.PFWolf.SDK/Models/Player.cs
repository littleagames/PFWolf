using LittleAGames.PFWolf.SDK.Components;

namespace LittleAGames.PFWolf.SDK.Models;

public class Player : Actor
{
    public Player(int tileX, int tileY, float angle) : base(tileX, tileY, angle)
    {
    }

    public Player(Position position, float angle) : base(position, angle)
    {
    }
}