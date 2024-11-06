// Initializes SDL.
using Engine;
using Engine.Managers;
using LittleAGames.PFWolf.FileManager;
using LittleAGames.PFWolf.FileManager.Constants.GamePacks;

InitGame();

// END

void InitGame()
{
    var fileLoader = new FileLoader();
    var gamePack = new Wolfenstein3DApogee();
    var directory = "D:\\Wolf3D_Games\\wolf3d-v1.4-apogee";
    var isValidPack = fileLoader.Validate(gamePack, directory);
    if (!isValidPack)
    {
        Console.WriteLine($"Pack: {gamePack.PackName} not found in directory: {directory}");
        return;
    }

    var assetManager = new AssetManager(fileLoader);

    assetManager.AddGamePack(gamePack, directory);
    assetManager.AddModPack("D:\\PFWolf-Assets", "pfwolf.pk3");

    Console.WriteLine($"Assets loaded: {assetManager.AssetCount}");
    // foreach (var pk3File in args.Select(x => x.EndsWith(".pk3") && isValidFilePath(x))
    // {
    // TODO: assetManager.AddModdedPack(
    // }
    new GameManager(assetManager).Start();
}