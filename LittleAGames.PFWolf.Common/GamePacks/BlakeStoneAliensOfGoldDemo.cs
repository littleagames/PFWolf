namespace LittleAGames.PFWolf.Common.GamePacks;

public class BlakeStoneAliensOfGoldDemo : GamePack
{
    public const string AudioHed = "audiohed.bs1";
    public const string AudioT = "audiot.bs1";
    public const string IAnim = "ianim.bs1";
    public const string SAnim = "sanim.bs1";
    public const string MapHead = "maphead.bs1";
    public const string MapTemp = "maptemp.bs1";
    public const string SVswap = "svswap.bs1";
    public const string VgaDict = "vgadict.bs1";
    public const string VgaGraph = "vgagraph.bs1";
    public const string VgaHead = "vgahead.bs1";
    public const string Vswap = "vswap.bs1";
    
    public override string PackName => "Blake Stone: Aliens of Gold Demo";
    
    protected override List<GamePackFile> Files => [
        new(AudioHed, "52a203285041846c9fd73e8947a64522"),
        new(AudioT, "144a201ef4bd5bd9989dc61d9250eeac"),
        new(IAnim, "b13edd1c227806dc1bc4874ea1bd606c"),
        new(MapHead, "4560fb7492c99f9fddde9cc7987fdc77"),
        new(MapTemp, "6532d4062fed1817b440684c41a21fb5"),
        new(SAnim, "24848d1955e61d7d5d88ece1e9aa1557"),
        new(SVswap, "d9dbfb903478cf6c7ede6af9afe3b4d3"),
        new(VgaDict, "78ea7678704503dbb453c59bffcb4b98"),
        new(VgaGraph, "f5dfd3f330af0274bf87cd08c2513d55"),
        new(VgaHead, "3d6973e5032bfaadefa6684367a26f77"),
        new(Vswap, "35f3d32bad5803b2d69bf3cff92da5ef")
    ];
    protected override List<GamePackFileLoader> FileLoaders =>
    [
    ];
}