using LittleAGames.PFWolf.SDK.Components;

namespace LittleAGames.PFWolf.SDK.Models;

public abstract class Actor : Component
{
    private float _fineAngle;
    
    protected Actor(int tileX, int tileY, float angle)
    {
        X = (tileX<<16)+(1<<16)/2;
        Y = (tileY<<16)+(1<<16)/2;
        FineAngle = angle;
    }

    public int X { get; set; }
    public int Y { get; set; }

    public float FineAngle
    {
        get => _fineAngle;
        set => _fineAngle = AngleUtilities.FixAngle(value);
    }

    public short Angle => (short)FineAngle;

    public short TileX => (short)(X >> 16);
    public short TileY => (short)(Y >> 16);
    
    public void Rotate(float deltaAngle)
    {
        FineAngle += deltaAngle;
    }

    /// <summary>
    /// Moves the actor toward or away from their current angle +/- given angle to allow side-stepping or pushing back without changing the facing angle of the actor
    /// </summary>
    /// <param name="speed">Strength of the movement</param>
    /// <param name="angle">Offset of the current facing angle of the actor</param>
    public void Move(float speed, float angle)
    {
        var pa = FineAngle + angle;
        var pdx = (float)Math.Cos(pa.ToRadians());
        var pdy = (float)-Math.Sin(pa.ToRadians());
        
        X += (int)(pdx * speed);
        Y += (int)(pdy * speed);
        
    }
}