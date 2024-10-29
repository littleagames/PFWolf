using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Interactivity;
using LittleAGames.PFWolf.FileManager;
using LittleAGames.PFWolf.Launcher.ViewModels;
using LittleAGames.PFWolf.SDK;

namespace LittleAGames.PFWolf.Launcher;

public partial class MainWindow : Window
{
    private List<KeyValuePair<GamePack, string>> _packFiles;
    
    public MainWindow()
    {
        InitializeComponent();
        GetGamePacks();
    }

    public void LaunchGameHandler(object? sender, RoutedEventArgs routedEventArgs)
    {
        // Save field into a json config file
        var selectedPack = GamePacks.Items.GetAt(GamePacks.SelectedIndex) as GamePackViewModel;
        if (selectedPack == null)
        {
            return;
        }

        var pack = _packFiles.FirstOrDefault(x => x.Key.Id == selectedPack.Id);
            SelectionDescription.Text = pack.Value;
       
            // TODO: Make a configuration manager for this
            // configManager.SaveGamePack(directory, gamepack type)
            // TODO: maybe then the SDK will use this, and have configManager.ReadCustom(key, value), SaveCustom(key,value)
                // These will have a prefix, or be in the "Custom: {}" so they cannot modify other values of the config.
        // string json = File.ReadAllText("config.json");
        // PFWolfConfiguration config = JsonSerializer.Deserialize<PFWolfConfiguration>(json);
        //
        // // Need path to look, and what pack to load in when engine starts
        // config.StartupGamePackPath = pack.Value;
        // config.StartupGamePack = pack.Key.GetType();
        //
        // string output = JsonSerializer.Serialize(config);
        // File.WriteAllText("config.json", output);

    }

    private void GamePackSelectionChangedHandler(object? sender, SelectionChangedEventArgs e)
    {
        var selectedPack = GamePacks.Items.GetAt(GamePacks.SelectedIndex) as GamePackViewModel;
        if (selectedPack != null)
        {
            var pack = _packFiles.FirstOrDefault(x => x.Key.Id == selectedPack.Id);
            SelectionDescription.Text = pack.Value;
        }
        
        // TODO: Do the click action
    }

    private void GetGamePacks()
    {
        var path = "D:\\Wolf3D_Games";
        _packFiles = new FileLoader().FindAvailableGames(path);
        // DataContext.PackFiles = _packFiles;
        GamePacks.ItemsSource = _packFiles.Select(x => new GamePackViewModel
        {
            Id = x.Key.Id,
            Name = x.Key.PackName
        }).OrderBy(x => x.Name);
    }
}