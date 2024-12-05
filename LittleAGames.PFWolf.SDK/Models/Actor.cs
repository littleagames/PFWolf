using LittleAGames.PFWolf.SDK.Components;

namespace LittleAGames.PFWolf.SDK.Models;

public abstract class Actor : Component
{
    public Actor(int tileX, int tileY, float angle)
    {
        Position = new Position(tileX, tileY);
        Angle = angle;
    }

    public Actor(Position position, float angle)
    {
        Position = position;
        Angle = angle;
    }
    public Position Position { get; private set; }

    public Position FinePosition => new Position(
        (Position.X << (int)Helpers.TILESHIFT) + (int)(Helpers.TILEGLOBAL / 2),
        (Position.Y << (int)Helpers.TILESHIFT) + (int)(Helpers.TILEGLOBAL / 2)
    );
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