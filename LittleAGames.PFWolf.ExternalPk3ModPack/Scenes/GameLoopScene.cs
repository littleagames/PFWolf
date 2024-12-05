[PfWolfScene("wolf3d:GameLoopScene")]
public class GameLoopScene : Scene
{
    private Camera _camera;
    private Renderer _renderer;
    private ViewPort _viewPort;
    private Map _map;
    private Player? _player;
    
    public GameLoopScene()
    {
        Components.Add(Background.Create(0x7f));
        Components.Add(Graphic.Create("statusbar", new Position(0, 160))); // TODO: Component (Statusbar), it'll be made of many components to draw things
    }

    public override void OnStart()
    {
        // TODO: Either I came from the NewGame, LoadGame, or Died (this is the restartgame "goto" in GameLoop
        
        // if new game
        // TODO: Get "mapassetname" from scene.DataContext
        // TODO: Change assetname for map "E1M1" or something defined in the asset
        _map = Map.Create("Wolf1 Map1");
        _camera = Camera.Create();
        _renderer = RaycastRenderer.Create(_camera, _map, 304*2, 144*2);//AutoMapRenderer.Create(_camera, _map); // TODO: Camera just tells position to start
        _viewPort = ViewPort.Create(8*2, 8*2, 304*2, 144*2, _renderer);
        Components.Add(_viewPort);
        Components.Add(_map);
        Components.Add(_camera);
    }

    public override void OnPreUpdate(float deltaTime)
    {
        // TODO: Set camera to player 0
        if (_camera.AttachedActor == null)
        {
            _player = _map.FindComponent<Player>();
            if (_player != null)
                _camera.Attach(_player);
        }

        // Do events
        return;
    }

    // private void MoveAutomap()
    // {
    //     if (Input.IsKeyDown(Keys.Equals))
    //     {
    //         _renderer.UpdateScale(_renderer.Scale + 1);
    //     }
    //     else if (Input.IsKeyDown(Keys.Minus))
    //     {
    //         _renderer.UpdateScale(_renderer.Scale - 1);
    //     }
    // }

    private void MovePlayer(float deltaTime)
    {
        if (_player == null)
            return;
        // TODO: "How fast does player move per frame?"
        // Player moves 35 units per frame (70 if running)
        
        const int runMove = 70;
        const int baseMove = 35;
        var controlX = 0;
        var controlY = 0;
        
        var delta = (Input.IsKeyDown(Keys.RightShift) ? runMove : baseMove);
        
        if (Input.IsKeyDown(Keys.Down))
        {
            controlY -= delta;
        }
        else if (Input.IsKeyDown(Keys.Up))
        {
            controlY += delta;
        }
        
        if (Input.IsKeyDown(Keys.Left))
        {
            controlX -= delta;
        }
        else if (Input.IsKeyDown(Keys.Right))
        {
            controlX += delta;
        }

        if (controlX != 0)
        {
            _player.Rotate(CalcAngleFromForce(controlX));
        }
    }

    private static float CalcAngleFromForce(float force)
    {
        // TODO: Apply force to a rate min/max per frame
        // TODO: Calculate # of degrees to change
        var degrees = force / 20;
        return degrees;
    }
    
    public override void OnUpdate(float deltaTime)
    {
        // TODO: "Normalize" controls
        MovePlayer(deltaTime);
        //MoveAutomap();
        // Do pushwalls
        // Do physics
        // Do Actors
        return;
    }
}