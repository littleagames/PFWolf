using LittleAGames.PFWolf.SDK.Abstract;
using LittleAGames.PFWolf.SDK.Assets;

namespace LittleAGames.PFWolf.SDK.FileLoaders;

public class Wolf3DMapFileLoader : BaseFileLoader
{
    private readonly string _mapHead;
    private readonly string _gameMap;

    public Wolf3DMapFileLoader(string directory, string mapHead, string gameMap) : base(directory)
    {
        _mapHead = mapHead;
        _gameMap = gameMap;
    }

    public override List<Asset> Load()
    {
        var mapHeaderFilePath = Path.Combine(Directory, _mapHead);
        var gameMapDataFilePath = Path.Combine(Directory, _gameMap);
        
        throw new NotImplementedException();
    }
}