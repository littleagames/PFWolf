using LittleAGames.PFWolf.SDK.Abstract;
using LittleAGames.PFWolf.SDK.Assets;

namespace LittleAGames.PFWolf.SDK.FileLoaders;

public class Wolf3DVswapFileLoader : BaseFileLoader
{
    private readonly string _vswap;

    public Wolf3DVswapFileLoader(string directory, string vswap) : base(directory)
    {
        _vswap = vswap;
    }

    public override List<Asset> Load()
    {
        var vswapFilePath = Path.Combine(Directory, _vswap);
        
        return [];
    }
}