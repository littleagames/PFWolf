using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LittleAGames.PFWolf.Launcher.ViewModels;

public class GamePackViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public override string ToString()
    {
        return Name;
    }
}