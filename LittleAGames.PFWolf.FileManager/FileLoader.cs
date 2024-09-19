namespace LittleAGames.PFWolf.FileManager;

public class FileLoader
{
    private static string path = "D:\\wolf3d-v1.4-activision";
    //private static string path = "D:\\wolf3d-v1.4-apogee";
    private static readonly string[] filesToFind = new[] { 
        "AUDIOT.WL6",
        "AUDIOHED.WL6",
        "GAMEMAPS.WL6",
        "MAPHEAD.WL6",
        "VGADICT.WL6",
        "VGAGRAPH.WL6",
        "VGAHEAD.WL6",
        "VSWAP.WL6"//, 
    //, "ecwolf.pk3"
    };

    /// <summary>
    /// Looks for the files the game (currently just the apogee version), and returns the paths
    /// TODO: Future, returns the files, paths, and an identifier so they can be branched at a later time
    /// TODO: Priority order loading as well, this might branch out to several loader/managers
    /// </summary>
    /// <returns></returns>
    public static Result<string[]> CheckForFiles()
    {
        var files = Directory.GetFiles(path);

        var filesNotFound = filesToFind.Where(foundFile => !files.Select(Path.GetFileName).Any(p2 => p2?.Equals(foundFile, StringComparison.InvariantCultureIgnoreCase) ?? false));

        if (filesNotFound.Any())
        {
            return Result.Failure<string[]>($"Files not found: {string.Join(", ", filesNotFound.Select(x => $"\"{x}\""))}");
        }

        return Result.Success(files);
    }
}
