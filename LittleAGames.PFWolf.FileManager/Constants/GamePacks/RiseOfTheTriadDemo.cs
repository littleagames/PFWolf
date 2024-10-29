namespace LittleAGames.PFWolf.FileManager.Constants.GamePacks;

public class RiseOfTheTriadDemo : GamePack
{
    public const string WadFile = "huntbgin.wad";
    
    public override string PackName => "Rise of the Triad Demo";
    
    protected override List<GamePackFile> Files => [
        new(WadFile, "37793500e3b1de2125a98604b69838e3")
    ];
}