[PfWolfScene("wolf3d:GameLoopScene")]
public class GameLoopScene : Scene
{
    private Camera _camera ;
    private Renderer _renderer;
    private ViewPort _viewPort;
    private Map _map;
    
    public GameLoopScene()
    {
        Components.Add(Background.Create(0x7f));
        Components.Add(Graphic.Create("statusbar", new Position(0, 160))); // TODO: Component (Statusbar), it'll be made of many components to draw things
        _camera = Camera.Create(0, 0);
    }

    public override void OnStart()
    {
        // TODO: Either I came from the NewGame, LoadGame, or Died (this is the restartgame "goto" in GameLoop
        
        // if new game
        // TODO: Get "mapassetname" from scene.DataContext
        // TODO: Change assetname for map "E1M1" or something defined in the asset
        _map = Map.Create("Wolf1 Map1");
        _renderer = AutoMapRenderer.Create(_camera, _map);
        _viewPort = ViewPort.Create(8, 8, 304, 144, _renderer);
        Components.Add(_viewPort);
        Components.Add(_map);
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