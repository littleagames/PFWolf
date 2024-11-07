namespace LittleAGames.PFWolf.Common.GamePacks;

public class RiseOfTheTriadDemo : GamePack
{
    public const string WadFile = "huntbgin.wad";
    
    public override string PackName => "Rise of the Triad Demo";
    
    protected override List<GamePackFile> Files => [
        new(WadFile, "37793500e3b1de2125a98604b69838e3")
    ];
    protected override List<GamePackFileLoader> FileLoaders =>
    [
    ];
}