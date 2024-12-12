using LittleAGames.PFWolf.SDK.Utilities;

namespace Engine.Managers;

public interface IMapManager
{
    void Initialize(MapComponent component);
    void Update(MapComponent component);
}

public class MapManager : IMapManager
{
    private readonly IAssetManager _assetManager;

    public MapManager(IAssetManager assetManager)
    {
        _assetManager = assetManager;
    }

    public void Initialize(MapComponent component)
    {
        if (component.GetType().IsAssignableFrom(typeof(Map)))
        {
            var map = (Map)component;
            var mapAsset = (MapAsset?)_assetManager.FindAsset(AssetType.Map, map.AssetName);
            if (mapAsset == null)
            {
                return;
            }
            
            map.Width = mapAsset.Width;
            map.Height = mapAsset.Height;
            map.Name = mapAsset.Name;

            // TODO: These might be better in an SDK thing
            // I want the modder to be able to add their things to this
            // I'll need to just have a "plane loader component" that takes in data, and the SDK piece translates it
            // This takes that component, and just loads them in
            // That component will also update them as well
            
            BuildWalls(map, mapAsset);
            BuildDoors(mapAsset);
            BuildActors(map, mapAsset);
            BuildStatics(mapAsset);
            BuildFlats(mapAsset);
            
        }
    }

    public void Update(MapComponent component)
    {
        // TODO: Handle map things here, like actor behaviors
        // Build this like the VideoManager "Update". How does that update translate a component to the video?
        // How does the map component translate to the map?
    }

    private void BuildWalls(Map map, MapAsset mapAsset)
    {
        Dictionary<string, byte[]> wallAssets = new();
        
        // TODO: Should I make a "MapRenderComponent" that takes MapComponent and RenderComponent, and translates between the two?
        // Currently the video manager runs AFTER the map manager (so i can manipulate the data at first
        //var wallPlane = mapAsset.PlaneData[0];
        map.Plane[0] = mapAsset.PlaneData[0].To2DArray(mapAsset.Width, mapAsset.Height);
        map.Plane[1] = mapAsset.PlaneData[1].To2DArray(mapAsset.Width, mapAsset.Height);
        
        for (var y = 0; y < mapAsset.Height; y++)
        for (var x = 0; x < mapAsset.Width; x++)
        {
            var wallNum = map.Plane[0][x, y];
            if (wallNum > 0 && wallNum < 90) // TODO: These will be in the mapdefs.json
            {
                var wallLight = $"WALL{((wallNum - 1) * 2):D5}";
                var wallDark = $"WALL{(((wallNum - 1) * 2) + 1):D5}";
                // TODO: Add to dictionary?
                var wallLightTexture = _assetManager.FindAsset(AssetType.Texture, wallLight);
                var wallDarkTexture = _assetManager.FindAsset(AssetType.Texture, wallDark);
                // TODO: Add these to a HashSet for assets to load
                map.Children.Add(new Wall
                {
                    X = x, // TileX
                    Y = y, // TileY
                    Data = wallLightTexture.RawData.To2DArray(64,64),
                    North = wallLightTexture.RawData,
                    South = wallLightTexture.RawData,
                    East = wallDarkTexture.RawData,
                    West = wallDarkTexture.RawData,
                });
            }
        }
        
        //var uniqueWalls = map.Plane[0].GroupBy(x => x); // TODO: This may not be necessary as the previous step will gather all of the required asset names
        // TODO: Map the wallPlane numbers to MapDefinitions
        // First can be a fake mapper (1 == WALL000001) (WallDefinition { N = {assetName}, E, S, W }
        // TODO: _assetManager.FindAssets(AssetType, List<string> assetNames)
        // TODO: add to ActiveMap? (object name for a fully built out map)
        // ActiveMap contains 2D array of "tilemap"
        //           contains 2D array of "statics"
        //           contains 2D array of "actors"
        // TODO: This is what will be put into a save game???
    }

    private void BuildDoors(MapAsset mapAsset)
    {
    }

    private void BuildActors(Map map, MapAsset mapAsset)
    {
        var objectsPlane = mapAsset.PlaneData[1];
        
        for (var y = 0; y < mapAsset.Height; y++)
        for (var x = 0; x < mapAsset.Width; x++)
        {
            var objectNum = objectsPlane[y * mapAsset.Width + x];
            SpriteAsset? automapPlayerArrow = _assetManager.FindAsset(AssetType.Sprite, "player-arrow") as SpriteAsset;
            switch (objectNum)
            {
                case 19: // North
                    map.Children.Add(new Player(x, y, 90.0f));
                    break;
                case 20: // East
                    map.Children.Add(new Player(x, y, 0.0f));
                    break;
                case 21: // South
                    map.Children.Add(new Player(x, y, 270.0f));
                    break;
                case 22: // West
                    map.Children.Add(new Player(x, y, 180.0f));
                    break;
            }
            if (automapPlayerArrow != null)
                map.Children.Add(new Sprite { Data = automapPlayerArrow.RawData });
        }
        //var uniqueObjects = objectsPlane.GroupBy(x => x);
        // Group
        // Find all assets
        // All of these numbers should translate to an object list defined in the pk3
        // Build as a list of objects, list of actors, etc
        // MapDefinitions in objects (actor, static, etc) eventually modders can add more things "Trigger"
    }
    
    private void BuildStatics(MapAsset mapAsset)
    {
    }
    
    private void BuildFlats(MapAsset mapAsset)
    {
        var flatsPlane = mapAsset.PlaneData[2];
        var uniqueFlats = flatsPlane.GroupBy(x => new
        {
            Ceiliing = x >> 8,
            Floor = x & 0xff
        });
    }
}