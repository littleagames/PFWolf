using LittleAGames.PFWolf.SDK.Components;

namespace LittleAGames.PFWolf.SDK.Handlers;

public class Wolf3dMapHandler : MapHandler
{
    public override void BuildMap(Map map, MapAsset mapAsset)
    {
        var wallPlane = mapAsset.PlaneData[0];
        var uniqueWalls = wallPlane.GroupBy(x => x);
        // TODO: Map the wallPlane numbers to MapDefinitions
            // First can be a fake mapper (1 == WALL000001) (WallDefinition { N = {assetName}, E, S, W }
        // TODO: _assetManager.FindAssets(AssetType, List<string> assetNames)
        // TODO: add to ActiveMap? (object name for a fully built out map)
            // ActiveMap contains 2D array of "tilemap"
            //           contains 2D array of "statics"
            //           contains 2D array of "actors"
            // TODO: This is what will be put into a save game???


        var objectsPlane = mapAsset.PlaneData[1];
        var uniqueObjects = objectsPlane.GroupBy(x => x);
        // Group
        // Find all assets
        // All of these numbers should translate to an object list defined in the pk3
        // Build as a list of objects, list of actors, etc
        // MapDefinitions in objects (actor, static, etc) eventually modders can add more things "Trigger"


        var flatsPlane = mapAsset.PlaneData[2];
        var uniqueFlats = flatsPlane.GroupBy(x => new
        {
            Ceiliing = x >> 8,
            Floor = x & 0xff
        });

        return;
    }
}