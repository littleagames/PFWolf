using LittleAGames.PFWolf.FileManager.Constants.GamePacks;

namespace LittleAGames.PFWolf.FileManager.Constants;

public class GamePackManager
{
    public GamePackManager()
    {
        GamePacks =
        [
            new OperationBodyCountFloppy(),
            new Wolfenstein3DShareware(),
            new Wolfenstein3DApogee(),
            new Wolfenstein3DActivision(),
            new SpearOfDestinyDemo()
        ];
    }
    
    public GamePack? Get(Guid id)
        => GamePacks.FirstOrDefault(x => x.Id == id);
    
    public IEnumerable<GamePack> FindGamePack(string file, string hash)
    {
        // TODO: Get Gamepacks by searching for "GamePacks" abstract class
        foreach (var pack in GamePacks)
        {
            var filename = pack.FindFileByHash(hash);
            if (!string.IsNullOrEmpty(filename) && filename.Equals(file, StringComparison.InvariantCultureIgnoreCase))
            {
                yield return pack;
            }
        }
    }
    
    // This collection should be private, and the only way to access is a "Get(GamePack gamePack)" method
    private List<GamePack> GamePacks { get; }
}