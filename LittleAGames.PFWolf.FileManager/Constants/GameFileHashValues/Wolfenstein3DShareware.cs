namespace LittleAGames.PFWolf.FileManager.Constants.GameFileHashValues;

public class Wolfenstein3DShareware : GamePack
{
    /// <summary>
    /// Wolfenstein 3D Shareware AUDIOHED.wl1
    /// </summary>
    public const string AudioHedMd5 = "58aa1b9892d5adfa725fab343d9446f8";

    /// <summary>
    /// Wolfenstein 3D Shareware file name for AUDIOHED.wl1
    /// </summary>
    public const string AudioHed = "audiohed.wl1";
    
    /// <summary>
    /// Wolfenstein 3D Shareware AUDIOT.wl1
    /// </summary>
    public const string AudioTMd5 = "4b6109e957b584e4ad7f376961f3887e";

    /// <summary>
    /// Wolfenstein 3D Shareware file name for AUDIOT.wl1
    /// </summary>
    public const string AudioT = "audiot.wl6";
    
    /// <summary>
    /// Wolfenstein 3D Shareware GAMEMAPS.wl1
    /// </summary>
    public const string GamemapsMd5 = "30fecd7cce6bc70402651ec922d2da3d";

    /// <summary>
    /// Wolfenstein 3D Shareware file name for GAMEMAPS.wl1
    /// </summary>
    public const string Gamemaps = "gamemaps.wl1";
    
    /// <summary>
    /// Wolfenstein 3D Shareware MAPHEAD.wl1
    /// </summary>
    public const string MapHeadMd5 = "7b6dd4e55c33c33a41d1600be5df3228";
        
    /// <summary>
    /// Wolfenstein 3D Shareware file name for MAPHEAD.wl1
    /// </summary>
    public const string MapHead = "maphead.wl1";
    
    /// <summary>
    /// Wolfenstein 3D Shareware VGADICT.wl1
    /// </summary>
    public const string VgaDictMd5 = "76a6128f3c0dd9b77939ce8313992746";
        
    /// <summary>
    /// Wolfenstein 3D Shareware file name for VGADICT.wl1
    /// </summary>
    public const string VgaDict = "vgadict.wl1";
    
    /// <summary>
    /// Wolfenstein 3D Shareware VGAGRAPH.wl1
    /// </summary>
    public const string VgaGraphMd5 = "74decb641b1a4faed173e10ab744bff0";

    /// <summary>
    /// Wolfenstein 3D Shareware file name for vgagraph.wl1
    /// </summary>
    public const string VgaGraph = "vgagraph.wl1";
    
    /// <summary>
    /// Wolfenstein 3D Shareware VGAHEAD.wl1
    /// </summary>
    public const string VgaHeadMd5 = "61bf1616e78367853c91f2c04e2c1cb7";
        
    /// <summary>
    /// Wolfenstein 3D Shareware file name for VGAHEAD.wl1
    /// </summary>
    public const string VgaHead = "vgahead.wl1";
    
    /// <summary>
    /// Wolfenstein 3D Shareware VSWAP.wl1
    /// </summary>
    public const string VswapMd5 = "6efa079414b817c97db779cecfb081c9";

    /// <summary>
    /// Wolfenstein 3D Shareware file name for VSWAP.wl1
    /// </summary>
    public const string Vswap = "vswap.wl1";

    public override GamePacks Pack => GamePacks.Wolfenstein3DShareware;

    protected override List<GamePackFile> Files => [
        new (AudioHed, AudioHedMd5),
        new (AudioT, AudioTMd5),
        new (Gamemaps, GamemapsMd5),
        new (MapHead, MapHeadMd5),
        new (VgaDict, VgaDictMd5),
        new (VgaGraph, VgaGraphMd5),
        new (VgaHead, VgaHeadMd5),
        new (Vswap, VswapMd5)
    ];
}