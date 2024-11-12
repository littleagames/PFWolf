﻿namespace LittleAGames.PFWolf.Common.Models;

public class UnpackedScript
{
    public string ScriptName { get; set; } = string.Empty;
    public byte[] RawData { get; set; } = [];
    public string Location { get; set; } = string.Empty;
}