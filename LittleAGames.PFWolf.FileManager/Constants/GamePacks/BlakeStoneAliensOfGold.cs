namespace LittleAGames.PFWolf.FileManager.Constants.GamePacks;

public class BlakeStoneAliensOfGold : GamePack
{
    public const string AudioHed = "audiohed.bs6";
    public const string AudioT = "audiot.bs6";
    public const string EAnim = "eanim.bs6";
    public const string GAnim = "ganim.bs6";
    public const string IAnim = "ianim.bs6";
    public const string SAnim = "sanim.bs6";
    public const string MapHead = "maphead.bs6";
    public const string MapTemp = "maptemp.bs6";
    public const string SVswap = "svswap.bs6";
    public const string VgaDict = "vgadict.bs6";
    public const string VgaGraph = "vgagraph.bs6";
    public const string VgaHead = "vgahead.bs6";
    public const string Vswap = "vswap.bs6";
    
    public override string PackName => "Blake Stone: Aliens of Gold";
    
    protected override List<GamePackFile> Files => [
        new(AudioHed, "52a203285041846c9fd73e8947a64522"),
        new(AudioT, "144a201ef4bd5bd9989dc61d9250eeac"),
        new(EAnim, "d4e0c268ec2464b92286bb75b355e5f7"),
        new(GAnim,"93dcd6239fff3f454b341108523bca80"),
        new(IAnim, "b13edd1c227806dc1bc4874ea1bd606c"),
        new(MapHead, "6cf51fb5cdece6b03c90feb6c94cc7ff"),
        new(MapTemp, "897af42e172bf6d68954c111d1625bd8"),
        new(SAnim, "24848d1955e61d7d5d88ece1e9aa1557"),
        new(SVswap, "d9dbfb903478cf6c7ede6af9afe3b4d3"),
        new(VgaDict, "b303d9155baaa1c126917053a93f054a"),
        new(VgaGraph, "6089a1cbe454cdf5022b3c115bcd2e9d"),
        new(VgaHead, "095d7c737e1c31d7674e1ed2160b676c"),
        new(Vswap, "5e7d5862fda72a1b2874301c01491aac")
    ];
    protected override List<GamePackFileLoader> FileLoaders =>
    [
    ];
}