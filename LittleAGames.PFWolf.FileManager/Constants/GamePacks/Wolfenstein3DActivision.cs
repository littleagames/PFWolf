namespace LittleAGames.PFWolf.FileManager.Constants.GamePacks;

public class Wolfenstein3DActivision : GamePack
{
    public const string AudioHed = "audiohed.wl6";
    public const string AudioT = "audiot.wl6";
    public const string Gamemaps = "gamemaps.wl6";
    public const string MapHead = "maphead.wl6";
    public const string VgaDict = "vgadict.wl6";
    public const string VgaGraph = "vgagraph.wl6";
    public const string VgaHead = "vgahead.wl6";
    public const string Vswap = "vswap.wl6";
    
    public override string PackName => "Wolfenstein 3D v1.4 Activision";
    protected override List<GamePackFile> Files => [
         new(AudioHed, "a41af25a2f193e7d4afbcc4301b3d1ce"),
         new(AudioT, "2385b488b18f8721633e5b2bdf054853"),
         new(Gamemaps, "a4e73706e100dc0cadfb02d23de46481"),
         new(MapHead, "b8d2a78bc7c50da7ec9ab1d94f7975e1"),
         new(VgaDict, "ccad1a688ebafad9856eca085a20dfc4"),
         new(VgaGraph, "f18b07d6ba988b8505415f7446235366"),
         new(VgaHead, "9059afb104a51140bd0c127b73717197"),
         new(Vswap, "a6d901dfb455dfac96db5e4705837cdb")
    ];
    protected override List<GamePackFileLoader> FileLoaders =>
    [
    ];
}