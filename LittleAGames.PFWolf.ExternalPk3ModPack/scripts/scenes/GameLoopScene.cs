﻿[PfWolfScene("wolf3d:GameLoopScene")]
public class GameLoopScene : Scene
{
    private Camera _camera;
    private Renderer _renderer;//, 
        //_automapRenderer, _rayCastRenderer;
    private ViewPort _viewPort;
    private Map _map;
    private Player? _player;
    private IngameConsole _ingameConsole;
    
    public GameLoopScene()
    {
        Components.Add(Background.Create(0x7f));
        Components.Add(Wolf3DStatusBar.Create(new Position(0, 160)));
        _ingameConsole = IngameConsole.Create(0, 0, 320, 100, 0x00);
    }

    public override void OnStart()
    {
        // TODO: Either I came from the NewGame, LoadGame, or Died (this is the restartgame "goto" in GameLoop
        
        // if new game
        // TODO: Get "mapassetname" from scene.DataContext
        // TODO: Change assetname for map "E1M1" or something defined in the asset
        _map = Map.Create("Wolf1 Map1");
        _camera = Camera.Create();
        _renderer = Wolf3DRaycastRenderer//RaycastRenderer
            .Create(_camera, _map, 640, 320);
        //_automapRenderer =AutoMapRenderer
        //    .Create(_camera, _map, 640, 320);
        //_renderer = _automapRenderer;
            
        _viewPort = ViewPort.Create(0, 0, 640, 320, _renderer);
        Components.Add(_viewPort);
        Components.Add(_map);
        Components.Add(_camera);
        Components.Add(_ingameConsole);
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
    
    private void MovePlayer(float deltaTime)
    {
        if (_player == null)
            return;
        // TODO: "How fast does player move per frame?"
        // Player moves 35 units per frame (70 if running)
        
        const int runMove = 300*70;
        const int baseMove = 150*70;
        var controlX = 0;
        var controlY = 0;
        
        var delta = (Input.IsKeyDown(Keys.RightShift) ? runMove : baseMove);
        
        if (Input.IsKeyDown(Keys.Down) || Input.IsKeyDown(Keys.S))
        {
            controlY -= delta;
        }
        else if (Input.IsKeyDown(Keys.Up)|| Input.IsKeyDown(Keys.W))
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

        if (Input.IsKeyDown(Keys.A)) // strafe left
        {
            _player.Move(delta, 90);
            Console.WriteLine($"Player -> Angle: {_player.Angle}, X: {_player.TileX}, Y: {_player.TileY}");
        }
        else if (Input.IsKeyDown(Keys.D)) // strafe right
        {
            _player.Move(delta, -90);
            Console.WriteLine($"Player -> Angle: {_player.Angle}, X: {_player.TileX}, Y: {_player.TileY}");
        }

        if (controlX != 0)
        {
            _player.Rotate(CalcAngleFromForce(controlX));
            Console.WriteLine($"Player -> Angle: {_player.Angle}, X: {_player.TileX}, Y: {_player.TileY}");
        }

        if (controlY != 0)
        {
            _player.Move(controlY, 0f);
            Console.WriteLine($"Player -> Angle: {_player.Angle}, X: {_player.TileX}, Y: {_player.TileY}");
        }
    }

    private static float CalcAngleFromForce(float force)
    {
        // TODO: Apply force to a rate min/max per frame
        // TODO: Calculate # of degrees to change
        var degrees = force / (70*100);
        return -degrees;
    }
    
    public override void OnUpdate(float deltaTime)
    {
        if (Input.IsKeyDown(Keys.Tilde))
        {
            _ingameConsole.ToggleState();
            Input.ClearKeysDown();
        }
        
        if (_ingameConsole.IsActive)
        {
            _ingameConsole.Listen(Input);
            return;
        }
        
        // TODO: "Normalize" controls
        MovePlayer(deltaTime);
        //MoveAutomap();
        // Do pushwalls
        // Do physics
        // Do Actors
        return;
    }
}