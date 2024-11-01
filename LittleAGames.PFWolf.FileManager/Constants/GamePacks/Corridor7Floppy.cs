namespace LittleAGames.PFWolf.FileManager.Constants.GamePacks;

public class Corridor7Floppy : GamePack
{
    public const string AudioHed = "audiohed.co7";
    public const string AudioMus = "audiomus.co7";
    public const string AudioSeq = "audioseq.co7";
    public const string AudioT = "audiot.co7";
    public const string GfxInfoV = "gfxinfov.co7";
    public const string GfxTiles = "gfxtiles.co7";
    public const string MapTemp = "maptemp.co7";
    public const string SeqTwo = "seqtwo.co7";
    public const string VgaDict = "vgadict.co7";
    public const string VgaGraph = "vgagraph.co7";
    public const string VgaHead = "vgahead.co7";
    
    public override string PackName => "Corridor 7 (Floppy Disk)";
    
    protected override List<GamePackFile> Files => [
        new(AudioHed, "793e3ae8bc64d623082db6bb1eca3f20"),
        new(AudioMus, "fd780c3a277c00ae2ebbb94430802d50"),
        new(AudioSeq, "ac792902adf37585855e5c579e079e0a"),
        new(AudioT, "c1789cc1aa02cdb31b7bd7c0af35a141"),
        new(GfxInfoV, "692631aa8c6e4842166f2676eb12cf1c"),
        new(GfxTiles, "7261ffedc794bbbec73933edb0cb9476"),
        new(MapTemp, "05791e398a36ead4d2c7389ea68047bb"),
        new(SeqTwo,"7e3436956361efc2c3b087369e85235d"),
        new(VgaDict, "0ec1ad2b9aa0e74092388eb7a04e10ce"),
        new(VgaGraph, "b3b64da4b725ae1cea119950594738b0"),
        new(VgaHead, "4037ffa97ca6afc3d6f604026d6b44ff")
    ];
    protected override List<GamePackFileLoader> FileLoaders =>
    [
    ];
}