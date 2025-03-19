using System.Numerics;

namespace LittleAGames.PFWolf.SDK.Components;

public class Map : MapComponent
{
    public string AssetName { get; }
    
    /// <summary>
    /// The raw IDs found in the game maps file
    /// </summary>
    public ushort[][,] PlaneIds { get; set; } = new ushort[3][,];

    /// <summary>
    /// Contains all complex tile data such as walls, doors, floor codes
    /// </summary>
    public MapComponent[][] TilePlane { get; set; }

    /// <summary>
    /// Contains a list of all physics bodies
    /// </summary>
    //public IList<PhysicsBox> PhysicsBodies { get; set; } = new List<PhysicsBox>();
    public PhysicsBox?[][] PhysicsMap;
    
    public IList<Actor> Actors { get; set; } = new List<Actor>();
    
    /// <summary>
    /// All loaded wall textures in the map
    /// </summary>
    public Dictionary<int, WallData> TileCache { get; set; } = new();
    
    /// <summary>
    /// All doors in the map
    /// </summary>
    public Dictionary<int, WallData> DoorCache { get; set; } = new();
    
    /// <summary>
    /// All doors in the map
    /// </summary>
    public Dictionary<string, SpriteData> SpriteCache { get; set; } = new();

    private Map(string assetName)
    {
        AssetName = assetName;
    }
    
    public static Map Create(string assetName)
        => new(assetName);
    
    public new int Width { get; set; }
    
    public new int Height { get; set; }
    
    /// <summary>
    /// Name given to the map for the game (e.g. "The Castle")
    /// </summary>
    public string Name { get; set; }

    private void OnPhysicsMove(PhysicsBox collider, Vector2 velocity) // TODO: PhysicsBox other
    {
        // TODO: Do physics check
        var body = PhysicsMap[(int)collider.X>>16][(int)collider.Y>>16];
        if (body != null && body != collider)
        {
            collider.Move(-velocity);
            Console.WriteLine($"Collision detected at {collider}");
        }
    }
    
    public override void OnStart()
    {
        // add all physicsbodies into a managed list here
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = TilePlane[y][x];
                if (tile is not (Wall or Door))
                    continue;
                if (tile.PhysicsBody != null)
                {
                    PhysicsMap[x][y] = tile.PhysicsBody; //PhysicsBox.Create(x << 16, y << 16, tile.Width, tile.Height);
                }
                //PhysicsBodies.Add();
            }
        }

        foreach (var actor in Actors)
        {
            if (actor.PhysicsBody != null)
            {
                PhysicsMap[actor.TileX][actor.TileY] = actor.PhysicsBody;
                actor.PhysicsBody.OnMove += OnPhysicsMove;
            }
            //PhysicsBodies.Add(PhysicsBox.Create(actor.X, actor.Y, actor.Width, actor.Height));
        }
    }

    public override void OnPreUpdate()
    {
        // for (var i = 0; i < PhysicsBodies.Count; i++)
        // {
        //     for (var j = 0; j < PhysicsBodies.Count; j++)
        //     {
        //         if (j == i) continue;
        //         if (PhysicsBodies[i].TileX == PhysicsBodies[j].TileX
        //             && PhysicsBodies[i].TileY == PhysicsBodies[j].TileY)
        //         {
        //             Console.WriteLine($"Collision at {PhysicsBodies[j].TileX}, {PhysicsBodies[j].TileY}");
        //         }
        //     }
        // }
        // foreach(var physicBox in PhysicsBodies)
        // {
        //     var colliding = PhysicsBodies.Where(x =>
        //         x != physicBox && x.TileX == physicBox.TileX && x.TileY == physicBox.TileY);
        //     if (!colliding.Any())
        //         continue;
        //
        //     Console.WriteLine($"Collision at {physicBox.TileX}, {physicBox.TileY}");
        //     // TODO: Found a collision, push physicsBox backwards? Should I also have a velocity that allows knowledge of direction and speed?
        // }
    }
    
    public override void OnUpdate()
    {
        foreach (var actor in Actors)
        {
            actor.Think();
            // TODO: Handle map things here, like actor behaviors   
        }
        // TODO: Map handling stuff?
        
        // Actors?
        // Pushwalls?
        
    }
}

public class Wall : MapComponent
{
    public int TileId { get; set; }
    public string North { get; init; }
    public string South { get; init; }
    public string East { get; init; }
    public string West { get; init; }
}
// public class Door : MapComponent
// {
//     public int TileId { get; set; }
//     public string North { get; init; }
//     public string South { get; init; }
//     public string East { get; init; }
//     public string West { get; init; }
// }

public class WallData
{
    public byte[] North { get; init; }
    public byte[] South { get; init; }
    public byte[] East { get; init; }
    public byte[] West { get; init; }
}

public class SpriteData : MapComponent
{
    public Position Offset { get; init; } = new(0, 0);
}