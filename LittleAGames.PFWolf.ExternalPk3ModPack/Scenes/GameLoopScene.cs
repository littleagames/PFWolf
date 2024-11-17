[PfWolfScene("wolf3d:GameLoopScene")]
public class GameLoopScene : Scene
{
    private PlayState _state = PlayState.Playing;
    private Camera _camera;
    public GameLoopScene()
    {
        _camera = Camera.Create(0,0,0);
        //var renderer = Render.Create()
        Components.Add(Background.Create(0x7f));
        Components.Add(Graphic.Create("statusbar", new Position(0, 160))); // TODO: Component (Statusbar), it'll be made of many components to draw things
        Components.Add(ViewPort.Create(8, 8, 304, 144, _camera));
        Components.Add(_camera);
        
        // TODO: Load the map
        LoadMap();
    }

    public override void OnPreUpdate()
    {
        // Check events
        SetupGameLevel();
    }

    /// <summary>
    /// Gets map
    /// </summary>
    public void LoadMap()
    {
        var mapAssetName = "Wolf1 Map1"; // TODO: MapAsset needs an identifier (this is just the name of the map), need a "MAP01" set, or "E1M1"
        Components.Add(Map.Create(mapAssetName));
        // Get map asset
        // Translate it to
        // - active walls
        // - Actors
        // - static sprites
        // - Doors
        // - Set ambush flags for actors
    }

    public void SetupGameLevel()
    {
        var rawMap = GetComponent<Map>();
        // Reset values in gamestate
        // Set RNG number
    }
}

internal enum PlayState
{
    Playing,
    Died,
    LevelCompleted,
    Victory,
    Paused,
    Warped
}