using System.IO.Compression;
using System.Reflection;
using LittleAGames.PFWolf.Common.Constants;
using LittleAGames.PFWolf.Common.Extensions;
using LittleAGames.PFWolf.Common.Models;
using LittleAGames.PFWolf.SDK.Abstract;
using LittleAGames.PFWolf.SDK.Assets;
using LittleAGames.PFWolf.SDK.Components;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Quantization;

namespace LittleAGames.PFWolf.Common.FileLoaders;

public class Pk3FileLoader : BaseFileLoader
{
    private readonly string _pk3File;
    private readonly string _pk3Directory;

    public Pk3FileLoader(string directory, string pk3File)
        : base(directory)
    {
        _pk3File = pk3File;
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
        var scriptAssets = new List<UnpackedScript>();
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

            if (entry.FullName.StartsWith("scripts/"))
            {
                if (entry.Name.EndsWith(".cs"))
                {
                    scriptAssets.Add(new UnpackedScript// TODO: This should be an "unpacked script" object that's not associated to Asset
                    {
                        ScriptName = CleanName(entry.Name),
                        RawData = rawData
                    });
                }
            }
        }

        // Bundle and validate scripts per pack

        assets.AddRange(CompileScripts(scriptAssets));
        
        return assets;
    }

    private List<ScriptAsset> CompileScripts(List<UnpackedScript> scripts)
    {
        var scriptAssets = new List<ScriptAsset>();
        List<SyntaxTree> syntaxes = new List<SyntaxTree>();
        foreach (var asset in scripts)
        {
            var code = System.Text.Encoding.Default.GetString(asset.RawData);
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            syntaxes.Add(syntaxTree);
        }

        var dd = typeof(Enumerable).GetTypeInfo().Assembly.Location;
        var coreDir = System.IO.Directory.GetParent(dd);

        var pfWolfSdkReference = AssemblyMetadata
            .CreateFromFile(Assembly.GetAssembly(typeof(RunnableBase)).Location)
            .GetReference();

        var compilation = CSharpCompilation.Create($"pfwolf-scripts-{CleanName(_pk3File)}.dll", syntaxes)
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .AddReferences(
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(coreDir.FullName + Path.DirectorySeparatorChar + "mscorlib.dll"),
                MetadataReference.CreateFromFile(coreDir.FullName + Path.DirectorySeparatorChar + "System.Runtime.dll"),
                pfWolfSdkReference
            );
        
        using (var ms = new MemoryStream())
        {
            // write IL code into memory
            EmitResult result = compilation.Emit(ms);

            // handle exceptions
            foreach (var diagnostic in result.Diagnostics)
            {
                // TODO: Report this in the console
                // Console.ResetColor();
                // if (diagnostic.Severity == DiagnosticSeverity.Error)
                //     Console.ForegroundColor = ConsoleColor.Red;
                // else if (diagnostic.Severity == DiagnosticSeverity.Warning)
                //     Console.ForegroundColor = ConsoleColor.DarkYellow;
                //
                // Console.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                // Console.ResetColor();
            }

            // Compiling is done for ALL scripts

            if (result.Success)
            {
                // load this 'virtual' DLL so that we can use
                ms.Seek(0, SeekOrigin.Begin);
                Assembly assembly = Assembly.Load(ms.ToArray());

                var allScripts = assembly.ExportedTypes.Where(s => s.IsSubclassOf(typeof(RunnableBase)));
                
                foreach (var runnable in allScripts)
                {
                    var attribute = runnable.GetCustomAttribute<PfWolfScriptAttribute>();
                    if (runnable.IsSubclassOf(typeof(Scene)))
                    {
                        scriptAssets.Add(new SceneAsset
                        {
                            Name = attribute?.ScriptName ?? runnable.Name,
                            AssetType = AssetType.ScriptScene,
                            Script = runnable
                        });
                    }
                    else
                    {
                        scriptAssets.Add(new ScriptAsset
                        {
                            Name = attribute?.ScriptName ?? runnable.Name,
                            AssetType = AssetType.Script,
                            Script = runnable
                        });
                    }
                }
            }
        }
        
        return scriptAssets;
    }

    private string CleanName(string pk3Name)
    {
        return Path.GetFileNameWithoutExtension(pk3Name);
    }
}
