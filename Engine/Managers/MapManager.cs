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
            map.TilePlane = new MapComponent[map.Height][];//map.Width];
            map.PhysicsMap = new PhysicsBox[map.Height][];
            for (var h = 0; h < map.Height; h++)
            {
                map.TilePlane[h] = new MapComponent[map.Width];
                map.PhysicsMap[h] = new PhysicsBox[map.Width];
            }
            
            // TODO: Fill each
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
        Dictionary<string, byte[]> tileAssets = new();
        
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

                map.TilePlane[y][x] = new Wall
                {
                    TileId = tileId,
                    North = definition.North,
                    South = definition.South,
                    East = definition.East,
                    West = definition.West
                };
                
                if (map.TileCache.ContainsKey(tileId))
                    continue; // already loaded
                
                foreach (var asset in assetNames)
                {
                    if (tileAssets.ContainsKey(asset))
                        continue; // already loaded
                    
                    var textureAsset = (WallAsset?)_assetManager.FindAsset(AssetType.Texture, asset);
                    if (textureAsset == null)
                    {
                        // texture not found
                    }
                    else
                    {
                        tileAssets.Add(asset, textureAsset.RawData);
                    }
                }
                
                map.TileCache.Add(tileId, new WallData
                {
                    North = tileAssets[definition.North],
                    South = tileAssets[definition.South],
                    East = tileAssets[definition.East],
                    West = tileAssets[definition.West],
                });
            }
        }
    }

    private void BuildDoors(Map map, MapAsset mapAsset, MapDefinitionAsset mapDefinitionAsset)
    {
        Dictionary<string, byte[]> doorAssets = new();
        
        for (var y = 0; y < mapAsset.Height; y++)
        for (var x = 0; x < mapAsset.Width; x++)
        {
            var tileId = map.PlaneIds[0][y, x];

            var doorDefinition = mapDefinitionAsset.FindDoor(tileId);
            
            // do same work but split to door asset cache
            if (doorDefinition != null)
            {
                map.TilePlane[y][x] = new Door
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
                var playerActor = new Player(x, y, GetAngleFromParams(playerData.Angle, playerData.Direction),
                    playerData.Health);
                //var playerPhysics = PhysicsBox.Create(x<<16, y<<16, 0x5800, 0x5800);
                // Hoping that this is a reference, and both update
                //map.PhysicsBodies.Add(playerPhysics);
                //playerActor.PhysicsBody = playerPhysics;//Children.Add(playerPhysics);

                map.Actors.Add(playerActor);
                map.Children.Add(playerActor);

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
            
            var partialActorDefinitions = _assetManager.GetAssets<ActorDefinitionAsset>(AssetType.ActorDefinitions);
            var fullActorDefinition = ActorDefinitionAsset.Merge(partialActorDefinitions);

            if (!fullActorDefinition.Actors.TryGetValue(actor.Actor, out var actorDefinition))
                continue;

            Actor? actorInstance;
            var script = _assetManager.FindAsset<ScriptAsset>(AssetType.Script, actor.Actor);
            if (script == null)
            {
                actorInstance = new WolfensteinActor(x,y);
            }
            else
            {
                var angle = GetAngleFromParams(actor.Angle, actor.Direction);
                actorInstance = Activator.CreateInstance(script.Script, [x,y, angle, actor.State]) as Actor;
            }

            if (actorInstance != null)
            {
                var mappedStates = actorDefinition.States
                    .Select(def =>
                        new KeyValuePair<string, IList<ActorState>>(def.Key, def.Value
                            .Select(st => new ActorState
                            {
                                Directional = st.Directional,
                                Action = st.Action,
                                AssetFrame = st.Frame,
                                Think = st.Think,
                                Ticks = st.Tics
                            }).ToList()))
                    .ToDictionary(d => d.Key, d => d.Value);
                actorInstance.ActorStates.CreateStates(mappedStates);

                var current = actorInstance.ActorStates.States.SelectMany(s =>
                    s.Value.SelectMany(f => GetAssetFrames(f.AssetFrame, f.Directional)));
                var spriteAssets = _assetManager.FindAssets<SpriteAsset>(AssetType.Sprite, current);

                foreach (var asset in spriteAssets)
                {
                    map.SpriteCache.TryAdd(asset.Name, new SpriteData
                    {
                        Offset = asset.Offset,
                        Width = asset.Width,
                        Height = asset.Height,
                        Data = asset.Pixels
                    });
                }
                
                map.Actors.Add(actorInstance);
            }
        }
    }

    private static IEnumerable<string> GetAssetFrames(string partialFrame, bool directional)
    {
        if (directional)
        {
            for (var index = 1; index <= 8; index++)
            {
                yield return $"{partialFrame}{index}";
            }
        }

        yield return $"{partialFrame}0";
    }
    
    private static float GetAngleFromParams(float? angle, string? direction)
    {
        if (angle != null)
        {
            return (float)Convert.ToDouble(angle);
        }

        if (direction != null)
        {
            var dirString = direction.ToUpperInvariant();
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