namespace LittleAGames.PFWolf.FileManager.Constants.GamePacks;

public class OperationBodyCountFloppy : GamePack
{
    public const string AudioHed = "audiohed.bc";
    public const string AudioMus = "audiomus.bc";
    public const string AudioT = "audiot.bc";
    public const string GfxTiles = "gfxtiles.bc";
    public const string MapHead = "maphead.bc";
    public const string MapTemp = "maptemp.bc";
    public const string VgaDict = "vgadict.bc";
    public const string VgaGraph = "vgagraph.bc";
    public const string VgaHead = "vgahead.bc";
    
    public override string PackName => "Operation Body Count (Floppy Disk)";
    
    protected override List<GamePackFile> Files => [
        new(AudioHed, "ccde3d43a536426426c4621d5fe0f370"),
        new(AudioMus, "95489aefee08b0d6262e434bfba1ca62"),
        new(AudioT, "078b7aaf594ae0d777b7bf72c6a3a700"),
        new(GfxTiles, "d8897fea1478375cb286939aab3f6466"),
        new(MapHead, "9c98aeaeb63fc21e66a4fbdb668420df"),
        new(MapTemp, "063176ca9fc0b0d8fd2784fab1cb14fb"),
        new(VgaDict, "9942425bb04c5009c7f1ce91234c7ee9"),
        new(VgaGraph, "e09be75f20afac2827fa6808ad35f0c7"),
        new(VgaHead, "f463da1212dc1238730028a21872da72")
    ];
}