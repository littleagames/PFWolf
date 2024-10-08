using LittleAGames.PFWolf.FileManager;

namespace LittleAGames.PFWolf.Connie;

internal class ConsoleFileClient
{
    private AudioFileManager audioFileManager = null!;
    private MapFileManager mapFileManager = null!;
    private VgaFileManager vgaFileManager = null!;
    private VswapFileManager swapFileManager = null!;
    private Pk3FileManager pk3FileManager = null!;

    public Result GetAudioHeaderList()
    {
        return GetFiles()
            .Bind(files =>
            {
                // Display audio file list of chunks
                audioFileManager = new AudioFileManager(
                                        audioFilePath: files.First(x => x.Contains("AUDIOT", StringComparison.InvariantCultureIgnoreCase)),
                                        audioHeaderFilePath: files.First(x => x.Contains("AUDIOHED", StringComparison.InvariantCultureIgnoreCase))
                                    );
                AnsiConsole.WriteLine("Get Audio Header List");
                AnsiConsole.WriteLine(new string('=', 50));
                var list = audioFileManager.GetAudioHeaderList();
                foreach (var item in list)
                {
                    AnsiConsole.WriteLine(item.Name);
                }

                return Result.Success();
            })
            .TapError(error => AnsiConsole.WriteLine(error));
    }

    public Result GetAudioDataList()
    {
        return GetFiles()
            .Bind(files =>
            {
                // Display audio file list of chunks
                audioFileManager = new AudioFileManager(
                                        audioFilePath: files.First(x => x.Contains("AUDIOT", StringComparison.InvariantCultureIgnoreCase)),
                                        audioHeaderFilePath: files.First(x => x.Contains("AUDIOHED", StringComparison.InvariantCultureIgnoreCase))
                                    );
                AnsiConsole.WriteLine("Get Audio Data List");
                AnsiConsole.WriteLine(new string('=', 50));
                var list = audioFileManager.GetAudioList();
                foreach (var item in list)
                {
                    AnsiConsole.WriteLine(item.Name);
                }

                return Result.Success();
            })
            .TapError(error => AnsiConsole.WriteLine(error));
    }

    public Result GetMapHeaderList()
    {
        return GetFiles()
            .Bind(files =>
            {
                // Display audio file list of chunks
                mapFileManager = new MapFileManager(
                                        mapFilePath: files.First(x => x.Contains("GAMEMAPS", StringComparison.InvariantCultureIgnoreCase)),
                                        mapHeaderFilePath: files.First(x => x.Contains("MAPHEAD", StringComparison.InvariantCultureIgnoreCase))
                                    );
                AnsiConsole.WriteLine("Get Map Header List");
                AnsiConsole.WriteLine(new string('=', 50));
                var list = mapFileManager.GetMapHeaderList();
                foreach (var item in list.MapHeaders)
                {
                    AnsiConsole.WriteLine(item.Display());
                }

                return Result.Success();
            })
            .TapError(error => AnsiConsole.WriteLine(error));
    }

    public Result GetMapDataList()
    {
        return GetFiles()
            .Bind(files =>
            {
                // Display audio file list of chunks
                mapFileManager = new MapFileManager(
                                        mapFilePath: files.First(x => x.Contains("GAMEMAPS", StringComparison.InvariantCultureIgnoreCase)),
                                        mapHeaderFilePath: files.First(x => x.Contains("MAPHEAD", StringComparison.InvariantCultureIgnoreCase))
                                    );
                AnsiConsole.WriteLine("Get Map Data List");
                AnsiConsole.WriteLine(new string('=', 50));
                var list = mapFileManager.GetMapDataList();
                foreach (var item in list)
                {
                    AnsiConsole.WriteLine(item.Name);
                }

                return Result.Success();
            })
            .TapError(error => AnsiConsole.WriteLine(error));
    }
    public Result GetVgaHuffmanTree()
    {
        return GetFiles()
            .Bind(files =>
            {
                // Display vga dictionary nodes
                vgaFileManager = new VgaFileManager(
                                        dictionaryFile: files.First(x => x.Contains("VGADICT", StringComparison.InvariantCultureIgnoreCase)),
                                        headerFile: files.First(x => x.Contains("VGAHEAD", StringComparison.InvariantCultureIgnoreCase)),
                                        dataFile: files.First(x => x.Contains("VGAGRAPH", StringComparison.InvariantCultureIgnoreCase))
                                    );
                AnsiConsole.WriteLine("Get VGA Dictionary Tree");
                AnsiConsole.WriteLine(new string('=', 50));
                var compression = vgaFileManager.GetDictionary();
                AnsiConsole.Write(compression.DisplayLeaves());

                return Result.Success();
            })
            .TapError(error => AnsiConsole.WriteLine(error));
    }

    public Result GetVgaHeaderList()
    {
        return GetFiles()
            .Bind(files =>
            {
                // Display vga dictionary nodes
                vgaFileManager = new VgaFileManager(
                                        dictionaryFile: files.First(x => x.Contains("VGADICT", StringComparison.InvariantCultureIgnoreCase)),
                                        headerFile: files.First(x => x.Contains("VGAHEAD", StringComparison.InvariantCultureIgnoreCase)),
                                        dataFile: files.First(x => x.Contains("VGAGRAPH", StringComparison.InvariantCultureIgnoreCase))
                                    );
                AnsiConsole.WriteLine("Get VGA Header");
                AnsiConsole.WriteLine(new string('=', 50));
                var list = vgaFileManager.GetHeaderList().Value;
                foreach (var item in list)
                {
                    AnsiConsole.WriteLine($"VGA #{list.IndexOf(item)} Offset: {item}");
                }

                return Result.Success();
            })
            .TapError(error => AnsiConsole.WriteLine(error));
    }

    public Result GetVgaDataList()
    {
        return GetFiles()
            .Bind(files =>
            {
                // Display vga dictionary nodes
                vgaFileManager = new VgaFileManager(
                                        dictionaryFile: files.First(x => x.Contains("VGADICT", StringComparison.InvariantCultureIgnoreCase)),
                                        headerFile: files.First(x => x.Contains("VGAHEAD", StringComparison.InvariantCultureIgnoreCase)),
                                        dataFile: files.First(x => x.Contains("VGAGRAPH", StringComparison.InvariantCultureIgnoreCase))
                                    );
                AnsiConsole.WriteLine("Get VGA Meta data");
                AnsiConsole.WriteLine(new string('=', 50));
                var vgaData = vgaFileManager.GetMetadataList().Value;
                //foreach (var item in list)
                //{
                    AnsiConsole.WriteLine($"VGA Num Chunks: {vgaData.NumChunks}");
                //}

                return Result.Success();
            })
            .TapError(error => AnsiConsole.WriteLine(error));
    }

    public Result GetVswapHeaderList()
    {
        return GetFiles()
            .Bind(files =>
            {
                // Display audio file list of chunks
                swapFileManager = new VswapFileManager(
                                        files.First(x => x.Contains("VSWAP", StringComparison.InvariantCultureIgnoreCase))
                                    );
                AnsiConsole.WriteLine("Get Swap Header Info");
                AnsiConsole.WriteLine(new string('=', 50));
                var swapData = swapFileManager.GetData().Value; // TODO: Result checking
                //foreach (var item in list)
                //{
                    AnsiConsole.WriteLine(swapData.Display());
                //}

                return Result.Success();
            })
            .TapError(error => AnsiConsole.WriteLine(error));
    }

    public Result GetVswapDataList()
    {
        return GetFiles()
            .Bind(files =>
            {
                // Display entries built out from page data
                swapFileManager = new VswapFileManager(
                                        files.First(x => x.Contains("VSWAP", StringComparison.InvariantCultureIgnoreCase))
                                    );
                AnsiConsole.WriteLine("Get Swap Header Info");
                AnsiConsole.WriteLine(new string('=', 50));
                var entries = swapFileManager.GetSwapEntries().Value; // TODO: Result checking
                foreach (var item in entries)
                {
                    AnsiConsole.WriteLine(item.Display());
                }

                return Result.Success();
            })
            .TapError(error => AnsiConsole.WriteLine(error));
    }

    public Result GetPk3DirectoryList()
    {
        return GetFiles()
            .Bind(files =>
            {
                // Display entries built out from page data
                pk3FileManager = new Pk3FileManager(
                                        files.First(x => x.Contains(".pk3", StringComparison.InvariantCultureIgnoreCase))
                                    );
                AnsiConsole.WriteLine("Get PK3 Directory Info");
                AnsiConsole.WriteLine(new string('=', 50));
                var entries = pk3FileManager.GetDirectoryList().Value; // TODO: Result checking
                foreach (var item in entries)
                {
                    AnsiConsole.WriteLine(item);
                }

                return Result.Success();
            })
            .TapError(error => AnsiConsole.WriteLine(error));
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
