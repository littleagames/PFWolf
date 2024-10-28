using LittleAGames.PFWolf.FileManager.Constants.GameFileHashValues;

namespace LittleAGames.PFWolf.FileManager.Constants;

public static class GameHashCheckValues
{
    public static IEnumerable<GamePack> FindGamePack(string file, string hash)
    {
        foreach (var pack in GamePacks)
        {
            var maybe = pack.FindFileByHash(hash);
            if (maybe.HasValue && maybe.Value.Equals(file, StringComparison.InvariantCultureIgnoreCase))
            {
                yield return pack;
            }
        }
    }
    
    // This collection should be private, and the only way to access is a "Get(GamePack gamePack)" method
    private static List<GamePack> GamePacks { get; } =
    [
        new Wolfenstein3DShareware(),
        new Wolfenstein3DApogee(),
        new Wolfenstein3DActivision()
    ];
}