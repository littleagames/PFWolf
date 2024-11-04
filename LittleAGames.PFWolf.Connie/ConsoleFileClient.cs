using LittleAGames.PFWolf.FileManager;
using LittleAGames.PFWolf.SDK.Assets;
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

    public Result GetMapHeader()
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
            Wolf3DMapFileLoader loader = gamePack.Key.GetLoader<Wolf3DMapFileLoader>(gamePack.Value);
            var headerData = loader.GetHeader();
            AnsiConsole.WriteLine("Get Map Header");
            AnsiConsole.WriteLine(new string('=', 50));
            
            AnsiConsole.WriteLine($"RLEW Tag: {headerData.RLEWTag:X2}");
            AnsiConsole.WriteLine($"Num Planes: {headerData.NumPlanes:N0}");
            AnsiConsole.WriteLine($"Num Available Maps: {headerData.NumAvailableMaps}");
            AnsiConsole.WriteLine($"Num Maps: {headerData.NumMaps}");

            for (var i = 0; i < headerData.HeaderOffsets.Length; i++)
            {
                AnsiConsole.WriteLine($"{i}] Offset: {headerData.HeaderOffsets[i]}");
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure($"Unable to run loader. Exception: {e.Message}");
        }
    }

    public Result GetMapHeaderSegments()
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
            Wolf3DMapFileLoader loader = gamePack.Key.GetLoader<Wolf3DMapFileLoader>(gamePack.Value);
            var headerSegments = loader.GetMapHeaderSegmentsList();
            AnsiConsole.WriteLine("Get Map Header Segments");
            AnsiConsole.WriteLine(new string('=', 50));
            
            for (var i = 0; i < headerSegments.Count; i++)
            {
                var segment = headerSegments[i];
                AnsiConsole.WriteLine($"{i}] \"{segment.Name}\" Size: ({segment.Width}x{segment.Height})");
                AnsiConsole.WriteLine($"\tPlane Starts: {string.Join(',', segment.PlaneStarts)}");
                AnsiConsole.WriteLine($"\tPlane Length: {string.Join(',', segment.PlaneLengths)}");
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure($"Unable to run loader. Exception: {e.Message}");
        }
    }

    public Result GetMapDataList()
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
            Wolf3DMapFileLoader loader = gamePack.Key.GetLoader<Wolf3DMapFileLoader>(gamePack.Value);
            var mapAssets = loader.Load();
            AnsiConsole.WriteLine("Get Map Assets");
            AnsiConsole.WriteLine(new string('=', 50));
            
            for (var i = 0; i < mapAssets.Count; i++)
            {
                var asset = (MapAsset)mapAssets[i];
                AnsiConsole.WriteLine($"{i}] \"{asset.Name}\" Size: ({asset.Width}x{asset.Height}), Planes: {asset.NumPlanes}");
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure($"Unable to run loader. Exception: {e.Message}");
        }
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
            Wolf3DVgaFileLoader loader = gamePack.Key.GetLoader<Wolf3DVgaFileLoader>(gamePack.Value);
            var headerList = loader.GetHeaderList();
            AnsiConsole.WriteLine("Get VGA Header Starts");
            AnsiConsole.WriteLine(new string('=', 50));
            
            for (var i = 0; i < headerList.Count; i++)
            {
                var segment = headerList[i];
                AnsiConsole.WriteLine($"{i}] {segment}");
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure($"Unable to run loader. Exception: {e.Message}");
        }
    }

    public Result GetVgaAssets()
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
            Wolf3DVgaFileLoader loader = gamePack.Key.GetLoader<Wolf3DVgaFileLoader>(gamePack.Value);
            var assets = loader.Load();
            AnsiConsole.WriteLine("Get VGA Assets");
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
