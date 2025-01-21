namespace LittleAGames.PFWolf.Common.GamePacks;

public class OperationBodyCountDemo : GamePack
{
    public const string AudioHed = "audiohed.co7";
    public const string AudioMus = "audiomus.co7";
    public const string AudioT = "audiot.co7";
    public const string GfxInfoV = "gfxinfov.co7";
    public const string GfxTiles = "gfxtiles.co7";
    public const string MapHead = "maphead.co7";
    public const string MapTemp = "maptemp.co7";
    public const string SeqSix = "seqsix.co7";
    public const string VgaDict = "vgadict.co7";
    public const string VgaGraph = "vgagraph.co7";
    public const string VgaHead = "vgahead.co7";
    
    public override string PackName => "operation-bc-demo";
    public override string PackDescription => "Operation Body Count Demo";
    
    protected override List<GamePackFile> Files => [
        new(AudioHed, "e023f57689119d8677b84a905996ef0b"),
        new(AudioMus, "fc1e9b47049a6fc480527a7baee12a15"),
        new(AudioT, "ddbfc6e711f6c91b7ed7274a0a35a1a8"),
        new(GfxInfoV, "1b501ef167445b37d4b31e84e707da62"),
        new(GfxTiles, "099d0404233e4c09985d6fee80c853c7"),
        new(MapHead, "43b089a0ba85f2ba9c7db50ea74d778b"),
        new(MapTemp, "10922affd8cbc4fcd7ba4665adc99f33"),
        new(SeqSix, "fc6e69c5daa0d1acbc675e404d2f3269"),
        new(VgaDict, "3d6e40bafc8922af5afcea2a668c364c"),
        new(VgaGraph, "924f11546256118a87b6b00cbff3f8f5"),
        new(VgaHead, "aca17a032ec23bfc9456ac31561d4646")
    ];
    protected override List<GamePackFileLoader> FileLoaders =>
    [
    ];
}