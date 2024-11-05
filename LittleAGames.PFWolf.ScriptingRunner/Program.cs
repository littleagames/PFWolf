namespace LittleAGames.PFWolf.ScriptingRunner;

using LittleAGames.PFWolf.SDK;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.IO.Compression;
using Microsoft.CodeAnalysis.CSharp;

internal class Program
{
    static async Task Main(string[] args)
    {
        //Test1();
        Test2();
    }

    static List<string> GetScripts()
    {
        var pk3File = "D:\\PFWolf-CSharp\\pk3-examples\\scripts-example.pk3";
        var codeScripts = new List<String>();
        using (ZipArchive archive = ZipFile.OpenRead(pk3File))
        {
            var scripts = archive.Entries.Where(x => !string.IsNullOrWhiteSpace(x.Name) && x.FullName.StartsWith("Scripts/", StringComparison.InvariantCultureIgnoreCase));
            foreach (var script in scripts)
            {
                using Stream stream = script.Open();
                using StreamReader reader = new StreamReader(stream);
                codeScripts.Add(reader.ReadToEnd());
            }
        }

        return codeScripts;
    }

    static void Test2()
    {
        Console.ResetColor();
        List<SyntaxTree> syntaxes = new List<SyntaxTree>();
        foreach (var code in GetScripts()) 
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            syntaxes.Add(syntaxTree);
        }

        var dd = typeof(Enumerable).GetTypeInfo().Assembly.Location;
        var coreDir = Directory.GetParent(dd);

        var pfWolfSdkReference = AssemblyMetadata
                .CreateFromFile(Assembly.GetAssembly(typeof(RunnableBase)).Location)
                .GetReference();

        var compilation = CSharpCompilation.Create("MyCustomScripts.dll", syntaxes)
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
                Console.ResetColor();
                if (diagnostic.Severity == DiagnosticSeverity.Error)
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (diagnostic.Severity == DiagnosticSeverity.Warning)
                    Console.ForegroundColor = ConsoleColor.DarkYellow;

                Console.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                Console.ResetColor();
            }

            // Compiling is done for ALL scripts

            if (result.Success)
            {
                // load this 'virtual' DLL so that we can use
                ms.Seek(0, SeekOrigin.Begin);
                Assembly assembly = Assembly.Load(ms.ToArray());

                // This can be stored in a ScriptsManager to be referenced at another time

                // Only pull scripts that are needed for the context

                

                var allScripts = assembly.ExportedTypes.Where(s => s.IsSubclassOf(typeof(RunnableBase)));

                //foreach (var runnable in foundTypes)
                //{
                // create instance of the desired class and call the desired function
                    //RunnableBase? obj = (RunnableBase?)Activator.CreateInstance(runnable);
                    //if (obj is null)
                    //    Console.WriteLine($"Could not instantiate script: {runnable.Name}");
                    //else
                    //{
                    //    Console.WriteLine($"Script found: {obj.GetType().Name}");
                    //    obj.Execute();
                    //}
                //}
            }
        }
        Console.ReadLine();
    }

    static void ReadScript(string code)
    {
        // TODO: Maybe this is where I could differentiate between types of scripts
        // This currently runs ALL of them in this moment
        var script = CSharpScript.Create<RunnableBase>(code, ScriptOptions.Default.WithReferences(Assembly.GetAssembly(typeof(RunnableBase))));

        using (var ms = new MemoryStream())
        {
            var compilation = script.GetCompilation();
            // write IL code into memory
            EmitResult result = compilation.Emit(ms);

            // handle exceptions
            foreach (var diagnostic in result.Diagnostics)
            {
                Console.ResetColor();
                if (diagnostic.Severity == DiagnosticSeverity.Error)
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (diagnostic.Severity == DiagnosticSeverity.Warning)
                    Console.ForegroundColor = ConsoleColor.DarkYellow;

                Console.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                Console.ResetColor();
            }

            if (result.Success)
            {
                // load this 'virtual' DLL so that we can use
                ms.Seek(0, SeekOrigin.Begin);
                Assembly assembly = Assembly.Load(ms.ToArray());
                var foundTypes = assembly.ExportedTypes.Where(s => s.IsSubclassOf(typeof(RunnableBase)));

                foreach (var runnable in foundTypes)
                {
                    // create instance of the desired class and call the desired function
                    RunnableBase? obj = (RunnableBase?)Activator.CreateInstance(runnable);
                    if (obj is null)
                        Console.WriteLine($"Could not instantiate script: {runnable.Name}");
                    else
                    {
                        Console.WriteLine($"Script found: {obj.GetType().Name}");
                        //obj.Execute();
                    }
                }
            }
        }
    }
}