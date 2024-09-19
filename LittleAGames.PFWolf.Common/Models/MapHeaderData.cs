using LittleAGames.PFWolf.Common.Utilities;
using System.Xml.Linq;

namespace LittleAGames.PFWolf.Common.Models;

public class MapHeaderList
{
    public List<MapHeaderData> MapHeaders { get; set; } = [];
    public ushort RLEWTag { get; set; }

    public int NumPlanes { get; set; } = 3;

    public byte[] HeaderData { get; set; } = [];
}

public class MapHeaderData
{
    public int Index { get; set; }
    public string Display() => $"Map {Index} (Pos: {MapHeaderSegmentPosition})";

    /// <summary>
    /// Position in the data file where the map header data is stored
    /// The plane data positions, sizes, etc are found from this data segment position
    /// </summary>
    public int MapHeaderSegmentPosition { get; set; }
}

public class MapHeaderMetadata
{
    public int[] PlaneStarts { get; set; } = [];
    public ushort[] PlaneLengths { get; set; } = [];
    public int Width { get; set; }
    public int Height { get; set; }
    public string Name { get; set; } = null!;

    public static MapHeaderMetadata Build(byte[] rawMetadata, int numPlanes)
    {
        MapHeaderMetadata metadata = new();

        var position = 0;
        
        var planeStartSize = sizeof(Int32) * numPlanes;
        metadata.PlaneStarts = Converters.ByteArrayToInt32Array(rawMetadata.Take(planeStartSize).ToArray(), numPlanes);
        position += planeStartSize;

        var planeLengthSize = sizeof(ushort) * numPlanes;
        metadata.PlaneLengths = Converters.ByteArrayToUInt16Array(rawMetadata.Skip(position).Take(planeLengthSize).ToArray(), numPlanes);
        position += planeLengthSize;

        var widthSize = sizeof(ushort);
        metadata.Width = BitConverter.ToUInt16(rawMetadata.Skip(position).Take(widthSize).ToArray());
        position += widthSize;

        var heightSize = sizeof(ushort);
        metadata.Height = BitConverter.ToUInt16(rawMetadata.Skip(position).Take(heightSize).ToArray());
        position += heightSize;

        var nameSize = Math.Max(rawMetadata.Length - position, sizeof(byte) * 16);
        var rawNameString = new string(rawMetadata.Skip(position).Select(x => (char)x).ToArray());
        metadata.Name = rawNameString.Substring(0, rawNameString.IndexOf('\0'));

        // or remainder of the byte array

        return metadata;
    }
}
