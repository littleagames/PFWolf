using LittleAGames.PFWolf.SDK.Components;

namespace LittleAGames.PFWolf.SDK.Models;

public abstract class Actor : Component
{
    public Actor(int x, int y, float angle)
    {
        Position = new Position(x, y);
        Angle = angle;
    }

    public Actor(Position position, float angle)
    {
        Position = position;
        Angle = angle;
    }
    public Position Position { get; private set; }
    
    public double Angle { get; private set; } = 0.0;

    public virtual void UpdatePosition(Position position)
    {
        Position = position;
    }
    
    public virtual void UpdatePosition(int x, int y)
    {
        Position = new Position(x, y);
    }

    /// <summary>
    /// Sets the angle of the actor to the given value
    /// </summary>
    /// <param name="angle"></param>
    public virtual void UpdateAngle(double angle)
    {
        Angle = angle;
    }

    /// <summary>
    /// Adjusts the angle of the actor +/- value given
    /// </summary>
    /// <param name="angle"></param>
    public virtual void Rotate(double angle)
    {
        Angle += angle;
        if (Angle > 360) Angle -= 360;
        if (Angle < 0) Angle += 360;
        
        Console.WriteLine($"Angle: {Angle}");
    }
}