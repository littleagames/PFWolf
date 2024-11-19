namespace LittleAGames.PFWolf.SDK.Models;

public abstract class Actor
{
    public virtual Position Position { get; private set; } = new(0,0);

    public virtual void UpdatePosition(Position position)
    {
        Position = position;
    }
    
    public virtual void UpdatePosition(int x, int y)
    {
        Position = new Position(x, y);
    }
}