using System.Reflection;

namespace Engine.Managers;

public interface IAssetManager
{
    void AddGamePack(GamePack gamePack, string directory);
    void AddModPack(string directory, string pk3File);
    void AddAssembly(Assembly assembly);
    Asset? FindAsset(AssetType assetType, string assetName);
    List<Asset> GetAssets(AssetType assetType);
}