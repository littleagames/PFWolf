using LittleAGames.PFWolf.FileManager;
using LittleAGames.PFWolf.SDK.FileLoaders;

namespace LittleAGames.PFWolf.Connie;

internal class ConsoleFileClient
{
    private KeyValuePair<GamePack, string>? chosenPack = null;
    public Result FindAvailableGames()
    {
        var path = "D:\\Wolf3D_Games";
        var availablePacks = new FileLoader().FindAvailableGames(path);
        chosenPack = UIBuilder.GamePackPicker(availablePacks);
        return Result.Success();
    }
    
    public Result GetAudioHeaderList()
    {
        if (!chosenPack.HasValue)
            return Result.Failure("No pack selected");
        
        var gamePack = chosenPack.Value;
        if (string.IsNullOrWhiteSpace(gamePack.Value) || !Path.Exists(gamePack.Value))
        {
            return Result.Failure($"Pack {gamePack.Value} not found in path: {gamePack.Value}");
        }

        try
        {
            // checks for that loader, returns loader here.
            Wolf3DAudioFileLoader loader = gamePack.Key.GetLoader<Wolf3DAudioFileLoader>(gamePack.Value);
            var headerData = loader.GetAudioHeaderList();
            AnsiConsole.WriteLine("Get Audio Header Data");
            AnsiConsole.WriteLine(new string('=', 50));
            foreach (var item in headerData)
            {
                AnsiConsole.WriteLine(item.Name);
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure($"Unable to run loader. Exception: {e.Message}");
        }
    }

    public Result GetAudioDataList()
    {
        if (!chosenPack.HasValue)
            return Result.Failure("No pack selected");
        
        var gamePack = chosenPack.Value;
        if (string.IsNullOrWhiteSpace(gamePack.Value) || !Path.Exists(gamePack.Value))
        {
            return Result.Failure($"Pack {gamePack.Value} not found in path: {gamePack.Value}");
        }
        try
        {
            // checks for that loader, returns loader here.
            Wolf3DAudioFileLoader loader = gamePack.Key.GetLoader<Wolf3DAudioFileLoader>(gamePack.Value);
            var assets = loader.Load();
            AnsiConsole.WriteLine("Get Audio Assets");
            AnsiConsole.WriteLine(new string('=', 50));
            foreach (var asset in assets)
            {
                AnsiConsole.WriteLine(asset.ToString());
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure($"Unable to run loader. Exception: {e.Message}");
        }
    }

    public Result GetMapHeaderList()
    {
        throw new NotImplementedException();
        // return GetFiles()
        //     .Bind(files =>
        //     {
        //         // Display audio file list of chunks
        //         mapFileManager = new MapFileManager(
        //                                 mapFilePath: files.First(x => x.Contains("GAMEMAPS", StringComparison.InvariantCultureIgnoreCase)),
        //                                 mapHeaderFilePath: files.First(x => x.Contains("MAPHEAD", StringComparison.InvariantCultureIgnoreCase))
        //                             );
        //         AnsiConsole.WriteLine("Get Map Header List");
        //         AnsiConsole.WriteLine(new string('=', 50));
        //         var list = mapFileManager.GetMapHeaderList();
        //         foreach (var item in list.MapHeaders)
        //         {
        //             AnsiConsole.WriteLine(item.Display());
        //         }
        //
        //         return Result.Success();
        //     })
        //     .TapError(error => AnsiConsole.WriteLine(error));
    }

    public Result GetMapDataList()
    {
        throw new NotImplementedException();
        // return GetFiles()
        //     .Bind(files =>
        //     {
        //         // Display audio file list of chunks
        //         mapFileManager = new MapFileManager(
        //                                 mapFilePath: files.First(x => x.Contains("GAMEMAPS", StringComparison.InvariantCultureIgnoreCase)),
        //                                 mapHeaderFilePath: files.First(x => x.Contains("MAPHEAD", StringComparison.InvariantCultureIgnoreCase))
        //                             );
        //         AnsiConsole.WriteLine("Get Map Data List");
        //         AnsiConsole.WriteLine(new string('=', 50));
        //         var list = mapFileManager.GetMapDataList();
        //         foreach (var item in list)
        //         {
        //             AnsiConsole.WriteLine(item.Name);
        //         }
        //
        //         return Result.Success();
        //     })
        //     .TapError(error => AnsiConsole.WriteLine(error));
    }
    public Result GetVgaHuffmanTree()
    {
        throw new NotImplementedException();
        // return GetFiles()
        //     .Bind(files =>
        //     {
        //         // Display vga dictionary nodes
        //         vgaFileManager = new VgaFileManager(
        //                                 dictionaryFile: files.First(x => x.Contains("VGADICT", StringComparison.InvariantCultureIgnoreCase)),
        //                                 headerFile: files.First(x => x.Contains("VGAHEAD", StringComparison.InvariantCultureIgnoreCase)),
        //                                 dataFile: files.First(x => x.Contains("VGAGRAPH", StringComparison.InvariantCultureIgnoreCase))
        //                             );
        //         AnsiConsole.WriteLine("Get VGA Dictionary Tree");
        //         AnsiConsole.WriteLine(new string('=', 50));
        //         var compression = vgaFileManager.GetDictionary();
        //         AnsiConsole.Write(compression.DisplayLeaves());
        //
        //         return Result.Success();
        //     })
        //     .TapError(error => AnsiConsole.WriteLine(error));
    }

    public Result GetVgaHeaderList()
    {
        throw new NotImplementedException();
        // return GetFiles()
        //     .Bind(files =>
        //     {
        //         // Display vga dictionary nodes
        //         vgaFileManager = new VgaFileManager(
        //                                 dictionaryFile: files.First(x => x.Contains("VGADICT", StringComparison.InvariantCultureIgnoreCase)),
        //                                 headerFile: files.First(x => x.Contains("VGAHEAD", StringComparison.InvariantCultureIgnoreCase)),
        //                                 dataFile: files.First(x => x.Contains("VGAGRAPH", StringComparison.InvariantCultureIgnoreCase))
        //                             );
        //         AnsiConsole.WriteLine("Get VGA Header");
        //         AnsiConsole.WriteLine(new string('=', 50));
        //         var list = vgaFileManager.GetHeaderList().Value;
        //         foreach (var item in list)
        //         {
        //             AnsiConsole.WriteLine($"VGA #{list.IndexOf(item)} Offset: {item}");
        //         }
        //
        //         return Result.Success();
        //     })
        //     .TapError(error => AnsiConsole.WriteLine(error));
    }

    public Result GetVgaDataList()
    {
        throw new NotImplementedException();
        // return GetFiles()
        //     .Bind(files =>
        //     {
        //         // Display vga dictionary nodes
        //         vgaFileManager = new VgaFileManager(
        //                                 dictionaryFile: files.First(x => x.Contains("VGADICT", StringComparison.InvariantCultureIgnoreCase)),
        //                                 headerFile: files.First(x => x.Contains("VGAHEAD", StringComparison.InvariantCultureIgnoreCase)),
        //                                 dataFile: files.First(x => x.Contains("VGAGRAPH", StringComparison.InvariantCultureIgnoreCase))
        //                             );
        //         AnsiConsole.WriteLine("Get VGA Meta data");
        //         AnsiConsole.WriteLine(new string('=', 50));
        //         var vgaData = vgaFileManager.GetMetadataList().Value;
        //         //foreach (var item in list)
        //         //{
        //             AnsiConsole.WriteLine($"VGA Num Chunks: {vgaData.NumChunks}");
        //         //}
        //
        //         return Result.Success();
        //     })
        //     .TapError(error => AnsiConsole.WriteLine(error));
    }

    public Result GetVswapHeaderList()
    {
        throw new NotImplementedException();
        // return GetFiles()
        //     .Bind(files =>
        //     {
        //         // Display audio file list of chunks
        //         swapFileManager = new VswapFileManager(
        //                                 files.First(x => x.Contains("VSWAP", StringComparison.InvariantCultureIgnoreCase))
        //                             );
        //         AnsiConsole.WriteLine("Get Swap Header Info");
        //         AnsiConsole.WriteLine(new string('=', 50));
        //         var swapData = swapFileManager.GetData().Value; // TODO: Result checking
        //         //foreach (var item in list)
        //         //{
        //             AnsiConsole.WriteLine(swapData.Display());
        //         //}
        //
        //         return Result.Success();
        //     })
        //     .TapError(error => AnsiConsole.WriteLine(error));
    }

    public Result GetVswapDataList()
    {
        throw new NotImplementedException();
        // return GetFiles()
        //     .Bind(files =>
        //     {
        //         // Display entries built out from page data
        //         swapFileManager = new VswapFileManager(
        //                                 files.First(x => x.Contains("VSWAP", StringComparison.InvariantCultureIgnoreCase))
        //                             );
        //         AnsiConsole.WriteLine("Get Swap Header Info");
        //         AnsiConsole.WriteLine(new string('=', 50));
        //         var entries = swapFileManager.GetSwapEntries().Value; // TODO: Result checking
        //         foreach (var item in entries)
        //         {
        //             AnsiConsole.WriteLine(item.Display());
        //         }
        //
        //         return Result.Success();
        //     })
        //     .TapError(error => AnsiConsole.WriteLine(error));
    }

    public Result GetPk3DirectoryList()
    {
        throw new NotImplementedException();
        // return GetFiles()
        //     .Bind(files =>
        //     {
        //         // Display entries built out from page data
        //         pk3FileManager = new Pk3FileManager(
        //                                 files.First(x => x.Contains(".pk3", StringComparison.InvariantCultureIgnoreCase))
        //                             );
        //         AnsiConsole.WriteLine("Get PK3 Directory Info");
        //         AnsiConsole.WriteLine(new string('=', 50));
        //         var entries = pk3FileManager.GetDirectoryList().Value; // TODO: Result checking
        //         foreach (var item in entries)
        //         {
        //             AnsiConsole.WriteLine(item);
        //         }
        //
        //         return Result.Success();
        //     })
        //     .TapError(error => AnsiConsole.WriteLine(error));
    }

    private static Result<string[]> GetFiles()
    {
        return FileLoader.CheckForFiles()
            .Ensure(files => files.Length > 0, "No files found")
            .Tap(files =>
            {
                // Display files found
                AnsiConsole.WriteLine($"Files found: {files.Length}");
                foreach (var file in files)
                {
                    AnsiConsole.WriteLine(file);
                }
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine(new string('=', 50));
                AnsiConsole.WriteLine();
            });
    }
}
