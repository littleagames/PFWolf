namespace LittleAGames.PFWolf.SDK.Scripting;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PfWolfSceneAttribute(string scriptName) : Attribute
{
    public string ScriptName { get; set; } = scriptName;
}