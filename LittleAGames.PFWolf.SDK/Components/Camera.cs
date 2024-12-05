namespace LittleAGames.PFWolf.SDK.Components;

public class Camera : Component
{
    public Position Position { get; private set; }
    public Position FinePosition { get; private set; }
    public double Angle { get; private set; }
    public double Pitch { get; private set; } = 0;
    public Actor? AttachedActor { get; private set; }
    
    private Camera(Actor attachedActor)
    {
        Position = new(attachedActor.Position.X, attachedActor.Position.Y);
        FinePosition = new(attachedActor.FinePosition.X, attachedActor.FinePosition.Y);
        AttachedActor = attachedActor;
    }
    
    private Camera(int x, int y)
    {
        Position = new(x, y);
        FinePosition = new(
            (Position.X << (int)Helpers.TILESHIFT) + (int)(Helpers.TILEGLOBAL / 2),
            (Position.Y << (int)Helpers.TILESHIFT) + (int)(Helpers.TILEGLOBAL / 2)
        );
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
        Position = new(AttachedActor.Position.X, AttachedActor.Position.Y);
        FinePosition = new(AttachedActor.FinePosition.X, AttachedActor.FinePosition.Y);
        Angle = actor.Angle;
    }

    public override void OnUpdate()
    {
        if (AttachedActor != null)
        {
            Position = new(AttachedActor.Position.X, AttachedActor.Position.Y);
            FinePosition = new(AttachedActor.FinePosition.X, AttachedActor.FinePosition.Y);
            Angle = AttachedActor.Angle;
        }
    }
}