using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace LittleAGames.PFWolf.Launcher;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        GamePacks.ItemsSource = new[]
        {
            "Wolfenstein 3D v1.4 Shareware",
            "Wolfenstein 3D v1.4 Apogee",
            "Wolfenstein 3D v1.4 Activision",
            "Spear of Destiny Demo",
            "Spear of Destiny v1.0",
            "Blake Stone: Aliens of Gold Shareware",
            "Blake Stone: Aliens of Gold",
            "Blake Stone: Planet Strike",
            "Operation Body Count Demo",
            "Operation Body Count (Floppy)",
            "Corridor 7 Demo",
            "Corridor 7 (Floppy)",
            "Corridor 7 (CD Version)"
        }.OrderBy(x => x);
    }

    public void LaunchGameHandler(object? sender, RoutedEventArgs routedEventArgs)
    {
        // TODO: Do the click action
        //GamePacks.se
    }

    private void GamePackSelectionChangedHandler(object? sender, SelectionChangedEventArgs e)
    {
        SelectionDescription.Text = GamePacks.Items.GetAt(GamePacks.SelectedIndex)?.ToString() ?? string.Empty;
        // TODO: Do the click action
    }
}