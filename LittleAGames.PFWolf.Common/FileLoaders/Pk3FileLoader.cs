﻿using System.IO.Compression;
using System.Reflection;
using LittleAGames.PFWolf.Common.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

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
        
        List<MapDefinitionAsset> allMapDefinitions = [];
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

            if (entry.FullName.StartsWith("sprites/"))
            {
                if (entry.Name.EndsWith(".png"))
                {
                    // TODO: Move this to a PNGDataLoader that might require a palette asset
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

                    assets.Add(new SpriteAsset
                    {
                        Name = CleanName(entry.Name),
                        RawData = graphicData,
                        Width = bmpImage.Width,
                        Height = bmpImage.Height,
                        Offset = new Position(bmpImage.Bounds.Top, bmpImage.Bounds.Left)
                    });
                }
                continue;
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
                        RawData = rawData,
                        Location = entry.FullName
                    });
                    continue;
                }
            }

            if (entry.FullName.StartsWith("gamepacks/"))
            {
                assets.Add(new GamePackDefinitionAsset(
                    name: CleanName(entry.Name),
                    rawData: rawData
                    // formatloaderjson
                ));
                continue;
                // TODO: Load all gamepacks, and match it with the selected game pack, and filter out the assets not needed
            }
            
            if (entry.FullName.StartsWith("mapdefs/"))
            {
                if (entry.Name.EndsWith(".json"))
                {
                    var mapDefinition = new MapDefinitionAsset (
                        CleanName(entry.FullName, includeDirectory: true),
                        rawData
                        // formatloaderjson
                    );
                    assets.Add(mapDefinition);
                    continue;
                }
                else if (entry.Name.EndsWith(".yml") || entry.Name.EndsWith(".yaml"))
                {
                    throw new NotSupportedException("YAML files currently not supported.");
                }
            }
            
            if (entry.FullName.StartsWith("actordefs/"))
            {
                if (entry.Name.EndsWith(".json"))
                {
                    var actorDefinition = new ActorDefinitionAsset (
                        CleanName(entry.FullName, includeDirectory: true),
                        rawData
                        // formatloaderjson
                    );
                    assets.Add(actorDefinition);
                    continue;
                }
                else if (entry.Name.EndsWith(".yml") || entry.Name.EndsWith(".yaml"))
                {
                    throw new NotSupportedException("YAML files currently not supported.");
                }
            }
        }

        // TODO: Compile list of map definitions
        
        // TODO: Validate mapDefinitions
        // if (mapDefinitions != null)
        // {
        //     assets.Add(mapDefinitions);
        // }
        
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
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code, path: asset.Location);
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
                MetadataReference.CreateFromFile(coreDir.FullName + Path.DirectorySeparatorChar + "System.Collections.dll"),
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
                if (diagnostic.Severity == DiagnosticSeverity.Error)
                    throw new Exception(diagnostic.GetMessage());
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
                
                scriptAssets = AssemblyScriptHelper.LoadScriptsFromAssembly(assembly);
            }
        }
        
        return scriptAssets;
    }

    private static string CleanName(string pk3Name, bool includeDirectory = false)
    {
        var fileName = Path.GetFileNameWithoutExtension(pk3Name);
        
        if (includeDirectory)
        {
            var directory = Path.GetDirectoryName(pk3Name);
            return $"{directory}/{fileName}";
        }

        return fileName;
    }
}
