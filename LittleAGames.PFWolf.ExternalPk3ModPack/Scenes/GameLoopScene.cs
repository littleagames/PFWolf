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
        _renderer = AutoMapRenderer.Create(_camera, _map);
        _viewPort = ViewPort.Create(8, 8, 304, 144, _renderer);
        Components.Add(_viewPort);
        Components.Add(_map);
    }

    public override void OnPreUpdate()
    {
        // Do events
        // TODO: "Normalize" controls
        MovePlayer();
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
        
        Console.WriteLine($"Scale: {_renderer.Scale}");
    }

    private void MovePlayer()
    {
        if (Input.IsKeyDown(Keys.Down))
        {
            _player.UpdatePosition(_player.Position.X, _player.Position.Y-1);
        }
        else if (Input.IsKeyDown(Keys.Up))
        {
            _player.UpdatePosition(_player.Position.X, _player.Position.Y+1);
        }
        
        if (Input.IsKeyDown(Keys.Left))
        {
            _player.UpdateAngle(_player.Angle+1);
        }
        else if (Input.IsKeyDown(Keys.Right))
        {
            _player.UpdateAngle(_player.Angle-1);
        }
        
        if (Input.IsKeyDown(Keys.A))
        {
            _player.UpdatePosition(_player.Position.X-1, _player.Position.Y-1);
        }
        else if (Input.IsKeyDown(Keys.D))
        {
            _player.UpdatePosition(_player.Position.X+1, _player.Position.Y+1);
        }
    }
    
    public override void OnUpdate()
    {
        // Do pushwalls
        // Do physics
        // Do Actors
        return;
    }
}