using LittleAGames.PFWolf.SDK.Components;

namespace LittleAGames.PFWolf.SDK.Models;

public abstract class Actor : Component
{
    public Position Position { get; private set; } = new(0,0);
    public double Angle { get; private set; } = 0.0;

    public virtual void UpdatePosition(Position position)
    {
        Position = position;
    }
    
    public virtual void UpdatePosition(int x, int y)
    {
        Position = new Position(x, y);
    }

    public virtual void UpdateAngle(double angle)
    {
        Angle = angle;
    }
}