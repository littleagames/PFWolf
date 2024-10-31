using Engine.Managers.AssetLoaders.Models;

namespace Engine.Managers.AssetLoaders;

public abstract class AssetLoader
{
    public abstract List<Asset> Get();
}