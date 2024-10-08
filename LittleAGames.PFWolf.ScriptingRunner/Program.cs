namespace LittleAGames.PFWolf.ScriptingRunner;

using LittleAGames.PFWolf.SDK;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Scripting;
using System.Buffers;
using System.Reflection;

internal class Program
{
    static async Task Main(string[] args)
    {
        //Test1();
        Test2();
    }

    static void Test1()
    {
        // define source code, then parse it (to the type used for compilation)
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(@"
                using LittleAGames.PFWolf.SDK;
                using System;

                public class TestScript : IRunnable
                {
                    public void Execute()
                    {
                        Console.WriteLine(""Test Script"");
                    }
                }");

        // define other necessary objects for compilation
        string assemblyName = Path.GetRandomFileName();
        MetadataReference[] references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IRunnable).Assembly.Location)
        };


        // analyse and generate IL code from syntax tree
        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using (var ms = new MemoryStream())
        {
            // write IL code into memory
            EmitResult result = compilation.Emit(ms);

            if (!result.Success)
            {
                // handle exceptions
                IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (Diagnostic diagnostic in failures)
                {
                    Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                }
            }
            else
            {
                // load this 'virtual' DLL so that we can use
                ms.Seek(0, SeekOrigin.Begin);
                Assembly assembly = Assembly.Load(ms.ToArray());

                // create instance of the desired class and call the desired function
                Type? type = assembly.GetType("LittleAGames.PFWolf.SDK");
                IRunnable? obj = (IRunnable?)Activator.CreateInstance(type);
                obj?.Execute();
            }
        }

        Console.ReadLine();
    }

    static void Test2()
    {
        var code = @"
                using LittleAGames.PFWolf.SDK;

                public class TestScript : IRunnable
                {
                    public void Execute()
                    {
                        System.Console.WriteLine(""Test Script"");
                    }
                }";

        var script = CSharpScript.Create<IRunnable>(code, ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(IRunnable))));
        script.Compile();
        using (var ms = new MemoryStream())
        {
            // write IL code into memory
            EmitResult result = script.GetCompilation().Emit(ms);

            if (!result.Success)
            {
                // handle exceptions
                IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (Diagnostic diagnostic in failures)
                {
                    Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                }
            }
            else
            {
                // load this 'virtual' DLL so that we can use
                ms.Seek(0, SeekOrigin.Begin);
                Assembly assembly = Assembly.Load(ms.ToArray());
                var foundTypes = assembly.ExportedTypes.Where(s => s.GetInterfaces().Any(si => si == typeof(IRunnable)));

                foreach (var runnable in foundTypes)
                {
                    // create instance of the desired class and call the desired function
                    IRunnable? obj = (IRunnable?)Activator.CreateInstance(runnable);
                    obj.Execute();
                }
            }
        }

        Console.ReadLine();
    }
}