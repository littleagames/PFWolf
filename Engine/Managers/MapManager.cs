namespace Engine.Managers;

public interface IMapManager
{
    void BuildMap(Map map);
}

public class MapManager : IMapManager
{
    private readonly IAssetManager _assetManager;
    private readonly MapHandler _mapHandler;

    public MapManager(IAssetManager assetManager, MapHandler mapHandler)
    {
        _assetManager = assetManager;
        _mapHandler = mapHandler;
    }

    public void BuildMap(Map map)
    {
        // TODO: _mapManager.BuildMap(map.AssetName)
        // TODO: MapBuilder (Wolf3DMapBuilder)
        // TODO: Asset.MapDefinition
        // MapDefinition translates map's plane[0] == 1 to 1 { wall001, wall002, wall001, wall002 }
        
        var mapAsset = (MapAsset?)_assetManager.FindAsset(AssetType.Map, map.AssetName);
        if (mapAsset == null)
        {
            return;
        }
        
        _mapHandler.BuildMap(map, mapAsset);
    }
}