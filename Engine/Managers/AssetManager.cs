using LittleAGames.PFWolf.FileManager;
using LittleAGames.PFWolf.SDK;
using LittleAGames.PFWolf.SDK.Assets;

namespace Engine.Managers;

public class AssetManager
{
    private readonly FileLoader _fileLoader;
    private readonly List<Asset> _assets = new List<Asset>();

    public AssetManager(FileLoader fileLoader)
    {
        _fileLoader = fileLoader;
    }
    
    public int AssetCount => _assets.Count;
    
    // TODO: This will take in a string "asset" and return the metadata, and byte data for that asset
    public void AddGamePack(GamePack gamePack, string directory)
    {
        var gamePackAssets = gamePack.LoadAssets(directory);
        _assets.AddRange(gamePackAssets);
    }

    public void AddModPack(object pk3File)
    {
        throw new NotImplementedException();
    }
    
    public List<Asset> GetAssets(AssetType assetType)
        => _assets.Where(a => a.AssetType == assetType).ToList();

    public Asset? FindAsset(AssetType assetType, string assetName)
    {
        return _assets.FirstOrDefault(a =>
            a.AssetType == assetType && a.Name.Equals(assetName, StringComparison.InvariantCultureIgnoreCase));
    }
}