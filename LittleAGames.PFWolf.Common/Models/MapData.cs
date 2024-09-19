using System.Xml.Linq;

namespace LittleAGames.PFWolf.Common.Models;

public class MapData
{
    public int Index { get; set; }
    public string Name { get => $"Map #{Index} \"{HeaderMetadata.Name}\", Size {HeaderMetadata.Width}x{HeaderMetadata.Height}"; }
    public byte[] MapMetadata { get; set; } = [];

    public MapHeaderMetadata HeaderMetadata { get; set; } = new();
}
