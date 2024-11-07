namespace LittleAGames.PFWolf.Common.GamePacks;

public class Corridor7CD : GamePack
{
    public const string AudioHed = "audiohed.co7";
    public const string AudioMus = "audiomus.co7";
    public const string AudioT = "audiot.co7";
    public const string GfxInfoV = "gfxinfov.co7";
    public const string GfxTiles = "gfxtiles.co7";
    public const string MapTemp = "maptemp.co7";
    public const string SeqFour = "seqfour.co7";
    public const string SeqOne = "seqone.co7";
    public const string SeqThree = "seqthree.co7";
    public const string VgaDict = "vgadict.co7";
    public const string VgaGraph = "vgagraph.co7";
    public const string VgaHead = "vgahead.co7";
    
    public override string PackName => "Corridor 7 CD";
    
    protected override List<GamePackFile> Files => [
        new(AudioHed, "6f41f714f882ea133dd5d1678448dffb"),
        new(AudioMus, "9ed47d8d50a5837af06a941d4d736d8c"),
        new(AudioT, "c925c5ac9ea51df814d3b6d9c5f2e771"),
        new(GfxInfoV, "6c6df783a32ebf7b1e6053f12010ec54"),
        new(GfxTiles, "e9a976a8456866d3c2cadd38c9f74662"),
        new(MapTemp, "f6f0c4400a8f003713479775823fd821"),
        
        // These files are empty, why?
        //new(SeqFour, "d41d8cd98f00b204e9800998ecf8427e"),
        //new(SeqOne, "d41d8cd98f00b204e9800998ecf8427e"),
        //new(SeqThree, "d41d8cd98f00b204e9800998ecf8427e"),
        new(VgaDict, "a0d2b334686bebdd3b1d9e79a91dc5b5"),
        new(VgaGraph, "3f6de286010058708110bc7000a2827c"),
        new(VgaHead, "a186ac0e094d4588c454e4f23c96a002")
    ];
    protected override List<GamePackFileLoader> FileLoaders =>
    [
    ];
}