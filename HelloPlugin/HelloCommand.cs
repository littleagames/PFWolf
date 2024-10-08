using LittleAGames.PFWolf.Editor.SDK;

namespace HelloPlugin;
/// <summary>
/// This plugin was developed for testing and example purposes.
/// 
/// <author>Little A Games</author>
/// <date>2024/10/07</date>
/// </summary>
public class HelloCommand : ICommand
{
    public string Name { get => "hello"; }
    public string Description { get => "Displays hello message."; }

    public int Execute()
    {
        Console.WriteLine("Hello !!!");
        return 0;
    }
}