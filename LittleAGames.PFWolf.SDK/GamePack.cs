namespace LittleAGames.PFWolf.SDK;

public record GamePackFile(string File, string Md5/*, Type GamePackFileLoader*/);

public abstract class GamePack
{
    public Guid Id = Guid.NewGuid();
    
    public abstract string PackName { get; }
    protected abstract List<GamePackFile> Files { get; }

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
}