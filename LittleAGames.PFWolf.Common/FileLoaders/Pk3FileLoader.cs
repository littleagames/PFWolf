using System.IO.Compression;
using LittleAGames.PFWolf.Common.Extensions;
using LittleAGames.PFWolf.SDK.Abstract;
using LittleAGames.PFWolf.SDK.Assets;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Quantization;

namespace LittleAGames.PFWolf.Common.FileLoaders;

public class Pk3FileLoader : BaseFileLoader
{
    private readonly string _pk3Directory;

    public Pk3FileLoader(string directory, string pk3File)
        : base(directory)
    {
        _pk3Directory = Path.Combine(directory, pk3File);
    }

    public List<string> GetDirectoryList()
    {
        var directories = new List<string>();
        using ZipArchive archive = ZipFile.OpenRead(_pk3Directory);
        foreach (ZipArchiveEntry entry in archive.Entries)
        {
            directories.Add(entry.FullName);
        }

        return directories;
    }
    
    public override List<Asset> Load()
    {
        using ZipArchive archive = ZipFile.OpenRead(_pk3Directory);
        var assets = new List<Asset>();
        foreach (ZipArchiveEntry entry in archive.Entries)
        {
            if (entry.Length == 0)
                continue;
            
            using var stream = entry.Open();
            byte[] rawData;
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                rawData = ms.ToArray();
            }

            if (entry.FullName.StartsWith("graphics/"))
            {
                if (entry.Name.EndsWith(".png"))
                {
                    using Image<Rgba32> bmpImage = Image.Load<Rgba32>(rawData);
                    var graphicData = new byte[bmpImage.Width * bmpImage.Height];
                    
                    // TODO: This is VERY slow
                    // Get a span for the current row of pixels
                    var pixelRowSpan = bmpImage.GetPixelMemoryGroup();
                    var index = 0;
                    foreach (var span in pixelRowSpan)
                    {
                        foreach (var pixel in span.Span)
                        {
                            graphicData[index++] =
                                ImageSharpExtensions.FindNearestColor(pixel, GamePalette.BasePalette.ToRgba32().ToList());
                        }
                    }

                    assets.Add(new GraphicAsset
                    {
                        Name = CleanName(entry.Name),
                        RawData = graphicData,
                        Dimensions = new Dimension((ushort)bmpImage.Width, (ushort)bmpImage.Height)
                    });
                }
                continue;
            }

            if (entry.FullName.StartsWith("palettes/"))
            {
                assets.Add(new PaletteAsset
                {
                    Name = CleanName(entry.Name),
                    RawData = rawData
                });
                continue;
            }
        }

        return assets;
    }

    private string CleanName(string pk3Name)
    {
        return Path.GetFileNameWithoutExtension(pk3Name);
    }
}
