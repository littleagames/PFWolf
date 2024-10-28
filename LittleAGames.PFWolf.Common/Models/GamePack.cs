using CSharpFunctionalExtensions;

namespace LittleAGames.PFWolf.Common.Models;

public record GamePackFile
{
    public GamePackFile(string file, string hash)
    {
        File = file;
        Hash = hash;
    }
    
    public string File { get; } = null!;
    public string Hash { get; } = null!;
}

public abstract class GamePack
{
    public abstract GamePacks Pack { get; }
    protected abstract List<GamePackFile> Files { get; }

    public Maybe<string> FindHashByFile(string file)
    {
        var result = Files.FirstOrDefault(x => x.File == file);
        if (result == null)
            return Maybe.None;

        return Maybe.From(result.Hash);
    }

    public Maybe<string> FindFileByHash(string hash)
       //=> Files.SingleOrDefault(x => x.Hash == hash).File;
    {
        var result = Files.FirstOrDefault(x => x.Hash == hash);
        if (result == null)
            return Maybe.None;

        return Maybe.From(result.File);
    }
}