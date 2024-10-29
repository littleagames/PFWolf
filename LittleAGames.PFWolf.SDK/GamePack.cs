namespace LittleAGames.PFWolf.SDK;

public record GamePackFile(string File, string Md5);

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
}