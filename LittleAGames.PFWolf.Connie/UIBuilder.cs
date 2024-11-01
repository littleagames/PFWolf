using Spectre.Console;

namespace LittleAGames.PFWolf.Connie;

internal class UIBuilder
{
    public static KeyValuePair<GamePack, string>? GamePackPicker(List<KeyValuePair<GamePack, string>> picks)
    {
        var pick = AnsiConsole.Prompt(
            new SelectionPrompt<KeyValuePair<GamePack, string>>()
                .Title(" Choice:")
                .AddChoices(picks)
                .UseConverter(x => $"{x.Key.PackName} in {x.Value}"));

        AnsiConsole.Clear();
        return pick;
    }
    public static void RenderMenu(Dictionary<int, (String Text, Func<Result> WorkTask)> picks)
    {
        while (true)
        {
            picks.ToList().ForEach(x =>
            {
                if (x.Key < 0)
                {
                    AnsiConsole.Write(new Padder(new Rule(x.Value.Text)
                    {
                        Justification = Justify.Left
                    }, new Padding(0, 1, 0, 0)));
                }
                else
                {
                    AnsiConsole.WriteLine($"{x.Key.ToString(),4}) {x.Value.Text}");
                }
            });

            AnsiConsole.WriteLine();

            var selection = AnsiConsole.Prompt(new TextPrompt<int>(" Your choice:")
                .ValidationErrorMessage("Pick A Number from the Available Options")
                .Validate(picks.ContainsKey)
            );

            AnsiConsole.Status().Start("Working...", _ => picks[selection].WorkTask.Invoke());

            PostExecutionPicks();
        }
    }

    private static void PostExecutionPicks()
    {
        var picks = new List<(string Text, Action Action)>
        {
            ("Keep Results", AnsiConsole.WriteLine),
            ("Clear Console", AnsiConsole.Clear)
        };

        var pick = AnsiConsole.Prompt(
            new SelectionPrompt<(string Text, Action Action)>()
            .Title(" Choice:")
            .AddChoices(picks)
            .UseConverter(x => x.Text));

        pick.Action.Invoke();
    }
}
