namespace LittleAGames.PFWolf.FileManager.Constants.GamePacks;

public class RiseOfTheTriad : GamePack
{
    public const string WadFile = "darkwar.wad";
    
    public override string PackName => "Rise of the Triad";
    
    protected override List<GamePackFile> Files => [
        new(WadFile, "e7bc1e06e6fa141e6601e64169f24697")
    ];
    protected override List<GamePackFileLoader> FileLoaders =>
    [
    ];
}