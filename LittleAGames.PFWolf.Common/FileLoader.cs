using System.Security.Cryptography;

namespace LittleAGames.PFWolf.Common;

public class FileLoader
{
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
