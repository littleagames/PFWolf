namespace LittleAGames.PFWolf.FileManager.Constants.GamePacks;

public class Wolfenstein3DAlpha : GamePack
{
    public const string AudioHed = "audiohed.wl1";
    public const string AudioT = "audiot.wl1";
    public const string HelpArt = "helpart.wl1";
    public const string MapTemp = "maptemp.wl1";
    public const string MapHead = "maphead.wl1";
    public const string OrderArt = "orderart.wl1";
    public const string VgaDict = "vgadict.wl1";
    public const string VgaGraph = "vgagraph.wl1";
    public const string VgaHead = "vgahead.wl1";
    public const string Vswap = "vswap.wl1";

    public override string PackName => "Wolfenstein 3D v1.0 Shareware";

    protected override List<GamePackFile> Files => [
        new (AudioHed, "178e747da0d6ad92a1551eca8e15d42c"),
        new (AudioT, "1b3a55cb26b390af3f3ccf636df554cf"),
        new (HelpArt,"e81708603948cd036e9856203bd091eb"),
        new (MapHead, "926ab17693b130ccc236c44518a425d6"),
        new (MapTemp, "f68e19c2b6ba6321cb8a0b9c5c02e780"),
        new (OrderArt,"60ee000af9529e92108a4bd567aead1a"),
        new (VgaDict, "a61ce00c9a890764d729f9415be71b7e"),
        new (VgaGraph, "9a9a6ac2bbdde462aa2a0a7e04e1d98f"),
        new (VgaHead, "58476125be7ec77afc8b93bfe12c50a7"),
        new (Vswap,  "9d75e163ad3dfec0534b45520c61c17c")
    ];
}