using System.Numerics;

namespace LittleAGames.PFWolf.SDK.Components;

public sealed class PhysicsBox : MapComponent
{
    private PhysicsBox(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public int TileX => X >> 16;
    public int TileY => Y >> 16;
    public event Action<PhysicsBox, Vector2>? OnMove;

    public static PhysicsBox Create(int tileX, int tileY, int width, int height)
    => new(tileX, tileY, width, height);

    public bool IsIntersectingWith(PhysicsBox other)
    {
        return (TileX < other.TileX + other.Width && TileX + Width > other.TileX &&
                TileY < other.TileY + other.Height && TileY + Height > other.TileY);
    }
    
    public void OnCollide(PhysicsBox other)
    {
        // Move player in opposite direction
    }

    public void Move(Vector2 velocity)
    {
        X += (int)velocity.X;
        Y += (int)velocity.Y;
        // TODO: Do physics here, and set the position based on any changes in velocity, or physics check
        OnMove?.Invoke(this, velocity);
        Console.WriteLine($"Physics Box at {TileX}, {TileY} moved.");
        // Event?
        // Event listened to by PhysicsManager
        // What allows the physics box to move? Without
        
        // Or does move do the work
        // and a collision detection just checks for collisions, spits you back out until you are at an appropriate X/Y?
            // This is done all in the same frame, so you shouldn't see zipping or snapping out of the wall
        //throw new NotImplementedException();
    }
}