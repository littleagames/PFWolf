using System.Security.Cryptography;

namespace LittleAGames.PFWolf.FileManager;

public class FileLoader
{
    //private static string path = "D:\\wolf3d-v1.4-activision";
    private static string _filePath = "D:\\wolf3d-v1.4-apogee";
     private static readonly string[] FilesToFind = new[] { 
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
         var files = Directory.GetFiles(_filePath);
    
         var filesNotFound = FilesToFind.Where(foundFile => !files.Select(Path.GetFileName).Any(p2 => p2?.Equals(foundFile, StringComparison.InvariantCultureIgnoreCase) ?? false)).ToList();
    
         if (filesNotFound.Count > 0)
         {
             return Result.Failure<string[]>($"Files not found: {string.Join(", ", filesNotFound.Select(x => $"\"{x}\""))}");
         }
    
         return Result.Success(files);
     }
    
    
    public bool Validate(GamePack gamePack, string path)
    {
        var foundFiles = FindAvailableGameFiles(path, true);
        return gamePack.Validate(foundFiles.Select(x => x.File).ToList());
    }

    public Dictionary<string, byte[]> Load(GamePack gamePack, string path)
    {
        var fileData = new Dictionary<string, byte[]>();
        var foundFiles = FindAvailableGameFiles(path, true);
        var dataFiles = gamePack.GetDataFiles(foundFiles);
        foreach (var gamePackFile in dataFiles)
        {
            var combinedPath = Path.Combine(gamePackFile.Path, gamePackFile.File);
            fileData.Add(gamePackFile.File, File.ReadAllBytes(combinedPath));
        }
        
        return fileData;
    }
    
    public List<KeyValuePair<GamePack, string>> FindAvailableGames(string path)
    {
        var foundFiles = FindAvailableGameFiles(path, false);
        
        // Iterate through the files and match them to possible game packs
        var gamePackFilesFound = new List<KeyValuePair<GamePack, string>>();
        var hashChecker = new GamePackManager();
        foreach (var directory in foundFiles.GroupBy(x => x.Path))
        {
            // For each file, find all unique gamepacks (distinct the list)
            var packFiles = new Dictionary<Guid, List<string>>();
            
            foreach (var file in directory)
            {
                foreach (var packFound in hashChecker.FindGamePack(file.File, file.Md5))
                {
                    if (packFiles.ContainsKey(packFound.Id))
                    {
                        packFiles[packFound.Id].Add(file.File);
                    }
                    else
                    {
                        packFiles.Add(packFound.Id, [file.File]);
                    }
                }
            }

            foreach (var foundPack in packFiles)
            {
                var gamePack = hashChecker.Get(foundPack.Key);
                if (gamePack == null)
                {
                    // TODO: This is a problem, but we'll ignore it for now
                    continue;
                }

                var isValidPack = gamePack.Validate(foundPack.Value);
                if (isValidPack)
                {
                    gamePackFilesFound.Add(new(gamePack, directory.Key));
                    // Add directory to returned model (so that game pack is associated to a directory
                }
            }
        }
        
        return gamePackFilesFound;
    }

    private List<DataFile> FindAvailableGameFiles(string path, bool topOnly)
    {
        var files = Directory.GetFiles(path, "*.*", topOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
        var foundFiles = new List<DataFile>();
        
        // List all files found in directory
        foreach (var file in files)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(file);

            var md5Hash = md5.ComputeHash(stream);
            var md5HashString = BitConverter.ToString(md5Hash).Replace("-", string.Empty).ToLowerInvariant();
            var fileInfo = new FileInfo(file);
            foundFiles.Add(new DataFile(fileInfo.Name, fileInfo.DirectoryName ?? string.Empty, md5HashString));
        }

        return foundFiles;
    }
}
