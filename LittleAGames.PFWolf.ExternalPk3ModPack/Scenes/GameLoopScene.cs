[PfWolfScene("wolf3d:GameLoopScene")]
public class GameLoopScene : Scene
{
    public GameLoopScene()
    {
        Components.Add(Background.Create(0x7f));
        Components.Add(Graphic.Create("statusbar", new Position(0, 160))); // TODO: Component (Statusbar), it'll be made of many components to draw things
    }

    public override void OnStart()
    {
    }

    public override void OnPreUpdate()
    {
        // Do events
        return;
    }

    public override void OnUpdate()
    {
        // Do pushwalls
        // Do physics
        // Do Actors
        return;
    }
}