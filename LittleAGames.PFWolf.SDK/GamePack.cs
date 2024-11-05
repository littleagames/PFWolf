using LittleAGames.PFWolf.SDK.Abstract;
using LittleAGames.PFWolf.SDK.Assets;

namespace LittleAGames.PFWolf.SDK;

public record GamePackFile(string File, string Md5);

public record GamePackFileLoader(Type FileLoader, List<string> AssetReferences, params string[] Files);

public abstract class GamePack
{
    public Guid Id = Guid.NewGuid();
    
    public abstract string PackName { get; }
    
    protected abstract List<GamePackFile> Files { get; }
    protected abstract List<GamePackFileLoader> FileLoaders { get; }

    public string? FindHashByFile(string file)
    {
        var result = Files.FirstOrDefault(x => x.File == file);
        return result?.Md5;
    }

    public string? FindFileByHash(string hash)
    {
        var result = Files.FirstOrDefault(x => x.Md5 == hash);
        return result?.File;
    }

    public bool Validate(List<string> files)
    {
        return Files.All(x => files.Any(f => f.Equals(x.File, StringComparison.InvariantCultureIgnoreCase)));
    }
    
    public IEnumerable<DataFile> GetDataFiles(List<DataFile> foundFiles)
    {
        var dataFiles = new List<DataFile>();
        foreach (var gamePackFile in Files)
        {
            var gameFile = foundFiles.FirstOrDefault(x => x.Md5.Equals(gamePackFile.Md5));
            if (gameFile == null)
            {
                throw new InvalidDataException($"Missing file: {gamePackFile.File} from found files.");
            }

            yield return gameFile;
        }
    }

    public T GetLoader<T>(string directory) where T : BaseFileLoader
    {
        var loader = FileLoaders.FirstOrDefault(x => x.FileLoader == typeof(T));
        return (T)InstantiateLoader(directory, loader);
    }

    private BaseFileLoader InstantiateLoader(string directory, GamePackFileLoader? loader)
    {
        if (loader == null)
            throw new InvalidOperationException($"Failed to instantiate loader.");

        try
        {
            List<object?> argsList = [directory, loader.AssetReferences];
            argsList.AddRange(loader.Files);
            BaseFileLoader fileLoader = Activator.CreateInstance(loader.FileLoader, argsList.ToArray()) as BaseFileLoader;
            if (fileLoader == null)
            {
                throw new InvalidOperationException($"File Loader {loader.GetType().Name} could not be instantiated.");
            }

            return fileLoader;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"File Loader {loader.GetType().Name} could not be instantiated.", ex);
        }
    }
    
    public IEnumerable<Asset> LoadAssets(string directory)
    {
        foreach (var loader in FileLoaders.Where(x => !typeof(BaseFileLoader).IsAssignableFrom(x.FileLoader)))
        {
            throw new InvalidOperationException($"Invalid FileLoader specified: {loader.GetType().Name}");
        }
        
        List<Asset> assets = new List<Asset>();
        foreach (var loader in FileLoaders)
        {
            var fileLoader = InstantiateLoader(directory, loader);
            
            var loadedAssets = fileLoader.Load();
            assets.AddRange(loadedAssets);
        }

        return assets;
    }
}