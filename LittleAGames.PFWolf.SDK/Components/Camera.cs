namespace LittleAGames.PFWolf.SDK.Components;

public class Camera : Component
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public short TileX => (short)(X >> 16);
    public short TileY => (short)(Y >> 16);
    
    public double Angle { get; private set; }
    public Actor? AttachedActor { get; private set; }
    
    private Camera(Actor attachedActor)
    {
        AttachedActor = attachedActor;
    }
    
    private Camera(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public static Camera Create(Actor attachedActor)
        => new Camera(attachedActor);

    public static Camera Create()
        => new Camera(0, 0);

    public static Camera Create(int x, int y)
        => new Camera(x, y);

    public void Attach(Actor actor)
    {
        AttachedActor = actor;
        X = actor.X;
        Y = actor.Y;
        Angle = actor.Angle;
    }

    public override void OnUpdate()
    {
        if (AttachedActor != null)
        {
            X = AttachedActor.X;
            Y = AttachedActor.Y;
            Angle = AttachedActor.Angle;
        }
    }
}