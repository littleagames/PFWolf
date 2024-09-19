namespace LittleAGames.PFWolf.Common.Models;

public class AudioData
{
    public int Index { get; set; }
    public string Name { get => $"Audio \"{AudioType}\" #{Index} (Size: {Size})"; }

    public byte[] Data { get; set; } = [];

    public int Size { get => Data.Length; }

    public AudioType AudioType { get; set; } = AudioType.Unknown;
}

public enum AudioType
{
    Unknown = 0,
    PcSound = 1,
    AdLibSound = 2,
    Music = 3
}