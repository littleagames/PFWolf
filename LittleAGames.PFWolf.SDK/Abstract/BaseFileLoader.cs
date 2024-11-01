using LittleAGames.PFWolf.SDK.Assets;

namespace LittleAGames.PFWolf.SDK.Abstract;

public abstract class BaseFileLoader
{
    protected readonly string Directory;

    protected BaseFileLoader(string directory)
    {
        Directory = directory;
    }
    
    public abstract List<Asset> Load();
}
