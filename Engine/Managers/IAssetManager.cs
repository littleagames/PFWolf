using LittleAGames.PFWolf.SDK;
using LittleAGames.PFWolf.SDK.Assets;

namespace Engine.Managers;

public interface IAssetManager
{
    void AddGamePack(GamePack gamePack, string directory);
    void AddModPack(string directory, string pk3File);
    Asset? FindAsset(AssetType assetType, string assetName);
    List<Asset> GetAssets(AssetType assetType);
}