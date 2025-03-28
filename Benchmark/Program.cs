// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using LittleAGames.PFWolf.Common;
using LittleAGames.PFWolf.Common.GamePacks;
using LittleAGames.PFWolf.Common.Managers;

// Need to benchmark draws
// Need to build all components to render quickly
// Maybe a way to keep the same byte array, and only overwrite data (instead of a new byte array each frame, if that's what's happening)


var fileLoader = new FileLoader();
var gamePack = new Wolfenstein3DApogee();
var directory = "D:\\projects\\Wolf3D\\Wolf3D_Games\\wolf3d-v1.4-apogee";
var isValidPack = fileLoader.Validate(gamePack, directory);
if (!isValidPack)
{
    Console.WriteLine($"Pack: {gamePack.PackDescription} not found in directory: {directory}");
    return;
}

IAssetManager assetManager = new AssetManager(fileLoader);

Stopwatch sw = new Stopwatch();
sw.Start();
assetManager.AddGamePack(gamePack, directory);
sw.Stop();
Console.WriteLine($"Loading base game pack in {sw.ElapsedMilliseconds}ms");
sw.Restart();
assetManager.AddModPack("D:\\projects\\Wolf3D\\PFWolf\\PFWolf-Assets", "pfwolf.pk3");
sw.Stop();
Console.WriteLine($"Loading pfwolf.pk3 pack in {sw.ElapsedMilliseconds}ms");
//assetManager.AddAssembly(typeof(LittleAGames.PFWolf.ExternalPk3ModPack.NoOp).Assembly);
Console.WriteLine($"Assets loaded: {assetManager.AssetCount}");