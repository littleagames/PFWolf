using Engine.Managers.AssetLoaders;
using Engine.Managers.AssetLoaders.Models;
using LittleAGames.PFWolf.FileManager;
using LittleAGames.PFWolf.SDK;

namespace Engine.Managers;

public class AssetManager
{
    private readonly FileLoader _fileLoader;
    private readonly List<Asset> _assets = new List<Asset>();

    public AssetManager(FileLoader fileLoader)
    {
        _fileLoader = fileLoader;
    }
    
    // TODO: This will take in a string "asset" and return the metadata, and byte data for that asset
    public void AddGamePack(GamePack gamePack, string directory)
    {
        var loadedGamePack = _fileLoader.Load(gamePack, directory);
        
        // Audio stuff
        loadedGamePack.TryGetValue("AUDIOHED.WL6", out var audioHedData); 
        loadedGamePack.TryGetValue("AUDIOT.WL6", out var audioTData);
        var audioAssets = new Wolf3DAudioAssetLoader(audioHedData, audioTData).Get();
        _assets.AddRange(audioAssets);
        
        loadedGamePack.TryGetValue("maphead.wl6", out var mapHeadData); 
        loadedGamePack.TryGetValue("gamemaps.wl6", out var gameMapsData);
        var mapAssets = new Wolf3DMapAssetLoader(mapHeadData, gameMapsData).Get();
        _assets.AddRange(mapAssets);

        loadedGamePack.TryGetValue("vswap.wl6", out var vswapData); 
        //var vSwapAssets = new Wolf3DVswapAssetLoader(vSwapData).Get();
        //_assets.AddRange(vSwapAssets);
        
        loadedGamePack.TryGetValue("vgadict.wl6", out var vgaDictData); 
        loadedGamePack.TryGetValue("vgahead.wl6", out var vgaHeadData); 
        loadedGamePack.TryGetValue("vgagraph.wl6", out var vgaGraphData);
        //var vgaAssets = new Wolf3DVgaDataAssetLoader(vgaDictData, vgaHeadData, vgaGraphData).Get();
        //_assets.AddRange(vgaAssets);

    }

    public void AddModPack(object pk3File)
    {
        throw new NotImplementedException();
    }
}