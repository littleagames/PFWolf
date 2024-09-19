namespace LittleAGames.PFWolf.Common.Models;

public class AudioHeaderData
{
    public int Index { get; set; }
    public string Name { get => $"Audio Header {Index} (Pos: {DataFilePosition})"; }

    /// <summary>
    /// Position in data file where chunk is stored
    /// </summary>
    public int DataFilePosition { get; set; }
}