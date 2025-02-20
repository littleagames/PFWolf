using System.Reflection;

namespace Engine.Managers;

public interface IAssetManager
{
    void AddGamePack(GamePack gamePack, string directory);
    void AddModPack(string directory, string pk3File);
    void AddAssembly(Assembly assembly);
    Asset? FindAsset(AssetType assetType, string assetName);
    T? FindAsset<T>(AssetType assetType, string assetName) where T : Asset;
    IEnumerable<T> FindAssets<T>(AssetType assetType, IEnumerable<string> assetNames) where T : Asset;
    List<Asset> GetAssets(AssetType assetType);
    
    List<T> GetAssets<T>(AssetType assetType) where T : Asset;
    
    public int AssetCount { get; }
}