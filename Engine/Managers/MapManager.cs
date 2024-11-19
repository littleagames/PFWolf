namespace Engine.Managers;

public interface IMapManager
{
    void Update(MapComponent component);
}

public class MapManager : IMapManager
{
    private readonly IAssetManager _assetManager;

    public MapManager(IAssetManager assetManager)
    {
        _assetManager = assetManager;
    }

    // public void BuildMap(Map map)
    // {
    //     // TODO: _mapManager.BuildMap(map.AssetName)
    //     // TODO: MapBuilder (Wolf3DMapBuilder)
    //     // TODO: Asset.MapDefinition
    //     // MapDefinition translates map's plane[0] == 1 to 1 { wall001, wall002, wall001, wall002 }
    //     
    //     var mapAsset = (MapAsset?)_assetManager.FindAsset(AssetType.Map, map.AssetName);
    //     if (mapAsset == null)
    //     {
    //         return;
    //     }
    // }

    public void Update(MapComponent component)
    {
        // TODO: Handle map things here, like actor behaviors
        // Build this like the VideoManager "Update". How does that update translate a component to the video?
        // How does the map component translate to the map?
    }
}