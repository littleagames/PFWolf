using LittleAGames.PFWolf.SDK.Abstract;
using LittleAGames.PFWolf.SDK.Assets;

namespace LittleAGames.PFWolf.SDK.FileLoaders;

public class Wolf3DVgaFileLoader : BaseFileLoader
{
    private readonly string _vgaDict;
    private readonly string _vgaHead;
    private readonly string _vgaGraph;

    public Wolf3DVgaFileLoader(string directory, string vgaDict, string vgaHead, string vgaGraph) : base(directory)
    {
        _vgaDict = vgaDict;
        _vgaHead = vgaHead;
        _vgaGraph = vgaGraph;
    }

    public override List<Asset> Load()
    {
        var vgaDictFilePath = Path.Combine(Directory, _vgaDict);
        var vgaHeadFilePath = Path.Combine(Directory, _vgaHead);
        var vgaGraphFilePath = Path.Combine(Directory, _vgaGraph);
        
        throw new NotImplementedException();
    }
}