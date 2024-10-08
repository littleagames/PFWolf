namespace LittleAGames.PFWolf.Editor.SDK;

public interface ICommand
{
    string Name { get; }
    string Description { get; }

    int Execute();
}
