namespace LittleAGames.PFWolf.FileManager.Constants.GamePacks;

public class BlakeStonePlanetStrike : GamePack
{
    public const string AudioHed = "audiohed.vsi";
    public const string AudioT = "audiot.vsi";
    public const string EAnim = "eanim.vsi";
    public const string IAnim = "ianim.vsi";
    public const string MapHead = "maphead.vsi";
    public const string MapTemp = "maptemp.vsi";
    public const string VgaDict = "vgadict.vsi";
    public const string VgaGraph = "vgagraph.vsi";
    public const string VgaHead = "vgahead.vsi";
    public const string Vswap = "vswap.vsi";
    
    public override string PackName => "Blake Stone 2: Planet Strike";
    
    protected override List<GamePackFile> Files => [
        new(AudioHed, "47c42494b67c14e93c7f3e90aed41a9a"),
        new(AudioT, "ce0ba53c4c41d0256896b1075653366e"),
        new(EAnim, "b9b3a25f54c3fb99fabb247e3713bb9a"),
        new(IAnim, "b13edd1c227806dc1bc4874ea1bd606c"),
        new(MapHead, "3c073394635250e348f6c1da6330069a"),
        new(MapTemp, "9dcbeab5d5c123455ae3c24964956b22"),
        new(VgaDict, "e20c862256e504194c5a9a68526a1beb"),
        new(VgaGraph, "ad57bd602ac4e571dd4101fe52b35807"),
        new(VgaHead, "31c0301eb320718029efaa3b275f689c"),
        new(Vswap,"24827f8b2920acc60bab0e57b6e9937e")
    ];
}