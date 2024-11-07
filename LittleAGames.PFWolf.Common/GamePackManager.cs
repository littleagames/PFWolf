using LittleAGames.PFWolf.Common.GamePacks;

namespace LittleAGames.PFWolf.Common;

public class GamePackManager
{
    public GamePackManager()
    {
        GamePacks =
        [
            new BlakeStoneAliensOfGold(),
            new BlakeStoneAliensOfGoldDemo(),
            new BlakeStonePlanetStrike(),
            new Corridor7CD(),
            new Corridor7Demo(),
            new Corridor7Floppy(),
            new OperationBodyCountDemo(),
            new OperationBodyCountFloppy(),
            new RiseOfTheTriad(),
            new RiseOfTheTriadDemo(),
            new SpearOfDestiny(),
            new SpearOfDestinyDemo(),
            new Wolfenstein3DAlpha(),
            new Wolfenstein3DShareware(),
            new Wolfenstein3DApogee(),
            new Wolfenstein3DActivision()
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