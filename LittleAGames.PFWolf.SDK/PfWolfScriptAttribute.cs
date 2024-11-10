namespace LittleAGames.PFWolf.SDK;

[AttributeUsage(AttributeTargets.Class)]
public class PfWolfScriptAttribute(string scriptName) : Attribute
{
    public string ScriptName { get; set; } = scriptName;
}