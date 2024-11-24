[PfWolfScene("wolf3d:GameLoopScene")]
public class GameLoopScene : Scene
{
    private Camera _camera ;
    private Renderer _renderer;
    private ViewPort _viewPort;
    private Map _map;
    private Player _player;
    
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
        _player = new Player();
        _player.UpdatePosition(29,57); // TODO: _map Player Position
        _camera = Camera.Create(_player);
        Components.Add(_camera);
        Components.Add(_player);
        _renderer = AutoMapRenderer.Create(_camera, _map);
        _viewPort = ViewPort.Create(8, 8, 304, 144, _renderer);
        Components.Add(_viewPort);
        Components.Add(_map);
    }

    public override void OnPreUpdate(float deltaTime)
    {
        // Do events
        // TODO: "Normalize" controls
        MovePlayer(deltaTime);
        MoveAutomap();
        return;
    }

    private void MoveAutomap()
    {
        if (Input.IsKeyDown(Keys.Equals))
        {
            _renderer.UpdateScale(_renderer.Scale + 0.1f);
        }
        else if (Input.IsKeyDown(Keys.Minus))
        {
            _renderer.UpdateScale(_renderer.Scale - 0.1f);
        }
    }

    private void MovePlayer(float deltaTime)
    {
        var tics = deltaTime * 1000;
        tics = Math.Max(tics, 10);
        tics = Math.Min(tics, -10);
        
        const int runMove = 70;
        const int baseMove = 35;
        var controlX = 0;
        var controlY = 0;
        int max, min;
        
        int delta = (int)(Input.IsKeyDown(Keys.RightShift) ? runMove * tics : baseMove * tics);
        
        if (Input.IsKeyDown(Keys.Down))
        {
            controlX -= delta;
        }
        else if (Input.IsKeyDown(Keys.Up))
        {
            controlX += delta;
        }
        
        if (Input.IsKeyDown(Keys.Left))
        {
            controlY -= delta;
        }
        else if (Input.IsKeyDown(Keys.Right))
        {
            controlY += delta;
        }
//
// bound movement to a maximum
//
        max = (int)(100 * tics);
        min = -max;
        if (controlX > max)
            controlX = max;
        else if (controlX < min)
            controlX = min;

        if (controlY> max)
            controlY = max;
        else if (controlY < min)
            controlY = min;
        if (controlX != 0 && controlY != 0)
        {
            Console.WriteLine($"ControlX: {controlX}, ControlY: {controlY}");
        }
        //_player.UpdatePosition(controlX, controlY);
        //
        // if (Input.IsKeyDown(Keys.A))
        // {
        //     _player.UpdatePosition(_player.Position.X-1, _player.Position.Y-1);
        // }
        // else if (Input.IsKeyDown(Keys.D))
        // {
        //     _player.UpdatePosition(_player.Position.X+1, _player.Position.Y+1);
        // }
        
        //Console.WriteLine($"Player X:{_player.Position.X} Y:{_player.Position.Y}");
    }
    
    public override void OnUpdate(float deltaTime)
    {
        // Do pushwalls
        // Do physics
        // Do Actors
        return;
    }
}