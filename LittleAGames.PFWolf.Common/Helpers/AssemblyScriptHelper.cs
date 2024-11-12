using System.Reflection;

namespace LittleAGames.PFWolf.Common.Helpers;

public class AssemblyScriptHelper
{
    public static List<ScriptAsset> LoadScriptsFromAssembly(Assembly assembly)
    {
        var scriptAssets = new List<ScriptAsset>();
        var allScripts = assembly.ExportedTypes.Where(s => s.IsSubclassOf(typeof(RunnableBase)));

        foreach (var runnable in allScripts)
        {
            var attribute = runnable.GetCustomAttribute<PfWolfSceneAttribute>();
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

        return scriptAssets;
    }
}