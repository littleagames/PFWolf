using System.Text.Json;
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
    private readonly string _mapDefinitionAssetName;

    public MapManager(IAssetManager assetManager, string mapDefinitionAssetName)
    {
        _assetManager = assetManager;
        _mapDefinitionAssetName = mapDefinitionAssetName;
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

            var mapDefinitionList = _assetManager.FindAsset<MapDefinitionAsset>(AssetType.MapDefinitions, _mapDefinitionAssetName);
            var partialMapDefinitions = _assetManager.GetAssets<MapDefinitionAsset>(AssetType.MapDefinitions)
                .Where(x => mapDefinitionList.MapDefinitions.Select(x => x).Contains(x.Name.Replace("\\", "/")));
            var fullMapDefinition = MapDefinitionAsset.Merge(partialMapDefinitions);
            
            map.Width = mapAsset.Width;
            map.Height = mapAsset.Height;
            map.TilePlane = new MapComponent[map.Height, map.Width];
            map.Name = mapAsset.Name;
            
            // TODO: Does this belong here?
            map.PlaneIds[0] = mapAsset.PlaneData[0].To2DArray(mapAsset.Width, mapAsset.Height);
            
            map.PlaneIds[1] = mapAsset.PlaneData[1].To2DArray(mapAsset.Width, mapAsset.Height);
            
            // TODO: These might be better in an SDK thing
            // I want the modder to be able to add their things to this
            // I'll need to just have a "plane loader component" that takes in data, and the SDK piece translates it
            // This takes that component, and just loads them in
            // That component will also update them as well
            
            BuildWalls(map, mapAsset, fullMapDefinition);
            BuildDoors(map, mapAsset, fullMapDefinition);
            BuildPlayers(map, mapAsset, fullMapDefinition);
            BuildActors(map, mapAsset, fullMapDefinition);
            BuildFlats(mapAsset);
        }
    }

    public void Update(MapComponent component)
    {
        // Build this like the VideoManager "Update". How does that update translate a component to the video?
        // How does the map component translate to the map?
    }

    private void BuildWalls(Map map, MapAsset mapAsset, MapDefinitionAsset mapDefinitionAsset)
    {
        Dictionary<string, byte[]> wallAssets = new();
        
        // TODO: Should I make a "MapRenderComponent" that takes MapComponent and RenderComponent, and translates between the two?
        // Currently the video manager runs AFTER the map manager (so i can manipulate the data at first
        //var wallPlane = mapAsset.PlaneData[0];
        
        for (var y = 0; y < mapAsset.Height; y++)
        for (var x = 0; x < mapAsset.Width; x++)
        {
            var tileId = map.PlaneIds[0][y, x];
            var definition = mapDefinitionAsset.FindWall(tileId);
            
            // do same work but split to door asset cache
            if (definition != null)
            {
                var assetNames = new HashSet<string>();
                assetNames.Add(definition.North);
                assetNames.Add(definition.South);
                assetNames.Add(definition.East);
                assetNames.Add(definition.West);

                map.TilePlane[y, x] = new Wall
                {
                    TileId = tileId,
                    North = definition.North,
                    South = definition.South,
                    East = definition.East,
                    West = definition.West
                };
                
                if (map.WallCache.ContainsKey(tileId))
                    continue; // already loaded
                
                foreach (var asset in assetNames)
                {
                    if (wallAssets.ContainsKey(asset))
                        continue; // already loaded
                    
                    var textureAsset = (WallAsset?)_assetManager.FindAsset(AssetType.Texture, asset);
                    if (textureAsset == null)
                    {
                        // texture not found
                    }
                    else
                    {
                        wallAssets.Add(asset, textureAsset.RawData);
                    }
                }
                
                map.WallCache.Add(tileId, new WallData
                {
                    North = wallAssets[definition.North],
                    South = wallAssets[definition.South],
                    East = wallAssets[definition.East],
                    West = wallAssets[definition.West],
                });
            }
        }
    }

    private void BuildDoors(Map map, MapAsset mapAsset, MapDefinitionAsset mapDefinitionAsset)
    {
        Dictionary<string, byte[]> doorAssets = new();
        
        // TODO: Should I make a "MapRenderComponent" that takes MapComponent and RenderComponent, and translates between the two?
        // Currently the video manager runs AFTER the map manager (so i can manipulate the data at first
        //var wallPlane = mapAsset.PlaneData[0];
        
        //map.PlaneIds[0] = mapAsset.PlaneData[0].To2DArray(mapAsset.Width, mapAsset.Height);
        //map.PlaneIds[1] = mapAsset.PlaneData[1].To2DArray(mapAsset.Width, mapAsset.Height);
        
        for (var y = 0; y < mapAsset.Height; y++)
        for (var x = 0; x < mapAsset.Width; x++)
        {
            var tileId = map.PlaneIds[0][y, x];

            var doorDefinition = mapDefinitionAsset.FindDoor(tileId);
            
            // do same work but split to door asset cache
            if (doorDefinition != null)
            {
                map.TilePlane[y, x] = new Door
                {
                    TileId = tileId,
                    North = doorDefinition.North,
                    South = doorDefinition.South,
                    East = doorDefinition.East,
                    West = doorDefinition.West
                };
                
                if (map.DoorCache.ContainsKey(tileId))
                    continue; // already loaded

                var assetNames = new HashSet<string>();
                assetNames.Add(doorDefinition.North);
                assetNames.Add(doorDefinition.South);
                assetNames.Add(doorDefinition.East);
                assetNames.Add(doorDefinition.West);

                foreach (var asset in assetNames)
                {
                    if (doorAssets.ContainsKey(asset))
                        continue; // already loaded
                    
                    var textureAsset = (WallAsset?)_assetManager.FindAsset(AssetType.Texture, asset);
                    if (textureAsset == null)
                    {
                        // texture not found
                    }
                    else
                    {
                        doorAssets.Add(asset, textureAsset.RawData);
                    }
                }
                
                map.DoorCache.Add(tileId, new WallData
                {
                    North = doorAssets[doorDefinition.North],
                    South = doorAssets[doorDefinition.South],
                    East = doorAssets[doorDefinition.East],
                    West = doorAssets[doorDefinition.West],
                });
            }
        }
    }

    private void BuildPlayers(Map map, MapAsset mapAsset, MapDefinitionAsset mapDefinitionAsset)
    {
        var objectsPlane = mapAsset.PlaneData[1];
        
        for (var y = 0; y < mapAsset.Height; y++)
        for (var x = 0; x < mapAsset.Width; x++)
        {
            var objectNum = objectsPlane[y * mapAsset.Width + x];
            var player = mapDefinitionAsset.Player;

            if (player.TryGetValue(objectNum, out var playerData))
            {
                map.Children.Add(new Player(x, y, GetAngleFromParams(playerData),
                    playerData.Health));

                float GetAngleFromParams(PlayerMapDefinition? actorParams)
                {
                    if (actorParams == null)
                        return 0;

                    if (actorParams.Angle != null)
                    {
                        return (float)Convert.ToDouble(actorParams.Angle);
                    }

                    if (actorParams.Direction != null)
                    {
                        var dirString = actorParams.Direction.ToString()?.ToUpperInvariant();
                        switch (dirString)
                        {
                            case "N":
                            case "NORTH":
                                return 90;
                            case "E":
                            case "EAST":
                                return 0;
                            case "S":
                            case "SOUTH":
                                return 270;
                            case "W":
                            case "WEST":
                                return 180;
                        }
                    }

                    throw new InvalidDataException("No valid direction parameters.");
                }
            }
        }
        //var uniqueObjects = objectsPlane.GroupBy(x => x);
        // Group
        // Find all assets
        // All of these numbers should translate to an object list defined in the pk3
        // Build as a list of objects, list of actors, etc
        // MapDefinitions in objects (actor, static, etc) eventually modders can add more things "Trigger"
    }
    
    private void BuildActors(Map map, MapAsset mapAsset, MapDefinitionAsset mapDefinitionAsset)
    {
        var objectsPlane = mapAsset.PlaneData[1];
        
        for (var y = 0; y < mapAsset.Height; y++)
        for (var x = 0; x < mapAsset.Width; x++)
        {
            var objectNum = (int)objectsPlane[y * mapAsset.Width + x];
            if (!mapDefinitionAsset.Actors.TryGetValue(objectNum, out var actor))
                continue;

          //  var actorType = Instantiate(actor.Actor);
          //  var actorDefinition = GetActorDefinition(actor.Actor);
            // TODO: Look for actor in scripts, if exists, instantiate as that actor type
            // TODO : If it doesn't exist, just instantiate as Actor
            
            
            map.Actors.Add(new Actor(x, y, 0)
            {
                //ActorStates = 
                //Hitpoints = actor.Hitpoints,
                
                // TODO: Set up actor, HP, type, etc
                // TODO: Set up states of the actor
            });
            //map.ObjectPlane[y, x];
            // TODO: Create actor, and add to actors list
            //actor.Spawn
        }
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