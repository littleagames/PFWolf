namespace LittleAGames.PFWolf.FileManager.Constants.GamePacks;

public class Corridor7Demo : GamePack
{
    public const string AudioDct = "audiodct.dmo";
    public const string AudioHed = "audiohed.dmo";
    public const string AudioMus = "audiomus.dmo";
    public const string AudioT = "audiot.dmo";
    public const string GfxInfoV = "gfxinfov.dmo";
    public const string GfxTiles = "gfxtiles.dmo";
    public const string MapHead = "maphead.dmo";
    public const string MapTemp = "maptemp.dmo";
    public const string MapTHead = "mapthead.dmo";
    public const string VgaDict = "vgadict.dmo";
    public const string VgaGraph = "vgagraph.dmo";
    public const string VgaHead = "vgahead.dmo";
    
    public override string PackName => "Corridor 7 Demo";
    
    protected override List<GamePackFile> Files => [
        new(AudioDct, "7af30a3a43dcffb32e8349952415c2e0"),
        new(AudioHed, "54d979dbffbd583c5158e5f56bac32da"),
        new(AudioMus, "c391cd1fd5b16c3c6bfe16a1d3855e91"),
        new(AudioT, "1cd477003fa46263f30e946fb8d3c014"),
        new(GfxInfoV, "ff46a434e431d0656c4348c02710d767"),
        new(GfxTiles, "a9981367aa77285c7c4a20b7190d0607"),
        new(MapHead, "5f375c1dd99472226b658a7682625e80"),
        new(MapTemp, "d43e1496f4651330433f52a00dcc6a11"),
        new(MapTHead, "df9e24c29208dd15cc95a0374344d625"),
        new(VgaDict, "730acad910b2b613c8c2de1601078096"),
        new(VgaGraph, "98a98eee69488b1ea69f6ed8b95e3dd9"),
        new(VgaHead, "cd64e59a1f7a88ff6c80bca7779dcf42")
    ];
    protected override List<GamePackFileLoader> FileLoaders =>
    [
    ];
}