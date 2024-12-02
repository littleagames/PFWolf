namespace LittleAGames.PFWolf.SDK.Components;

public class Camera : Component
{
    public Position Position { get; private set; }
    public Actor? AttachedActor { get; private set; }
    
    private Camera(Actor attachedActor)
    {
        Position = new(attachedActor.Position.X, attachedActor.Position.Y);
        AttachedActor = attachedActor;
    }
    
    private Camera(int x, int y)
    {
        Position = new(x, y);
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
    }

    public override void OnUpdate()
    {
        if (AttachedActor != null)
        {
            Position = new(AttachedActor.Position.X, AttachedActor.Position.Y);
        }
    }
}