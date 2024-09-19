using System.IO.Compression;

namespace LittleAGames.PFWolf.FileManager;

public class Pk3FileManager
{
    private readonly string pk3File;

    public Pk3FileManager(string pk3File)
    {
        this.pk3File = pk3File;
    }

    public Result<List<string>> GetDirectoryList()
    {
        var directories = new List<string>();
        using (ZipArchive archive = ZipFile.OpenRead(pk3File))
        {
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                directories.Add(entry.FullName);
            }
        }

        return directories;
    }
}
