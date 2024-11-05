using LittleAGames.PFWolf.SDK.Abstract;
using LittleAGames.PFWolf.SDK.Assets;
using LittleAGames.PFWolf.SDK.Compression;
using LittleAGames.PFWolf.SDK.Utilities;

namespace LittleAGames.PFWolf.SDK.FileLoaders;

public class Wolf3DMapFileLoader : BaseFileLoader
{
    private readonly List<string> _assetReferences;
    private readonly byte[] _headerData;
    private readonly byte[] _gameMapData;
    private const int DefaultNumberOfPlanes = 3;
    private const int DefaultMapNameSize = 16;

    public Wolf3DMapFileLoader(string directory, List<string> assetReferences, string mapHead, string gameMap) : base(directory)
    {
        _assetReferences = assetReferences;
        var mapHeaderFilePath = Path.Combine(Directory, mapHead);
        _headerData = File.ReadAllBytes(mapHeaderFilePath);

        var gameMapsFilePath = Path.Combine(Directory, gameMap);
        _gameMapData = File.ReadAllBytes(gameMapsFilePath);
    }

    public MapHeader GetHeader()
    {
        var position = 0;
        
         var rlewTag = BitConverter.ToUInt16(_headerData, 0);
         position += sizeof(ushort);
         
         // Original map seems to have 100 maps placed
         var offsetLength = (_headerData.Length - position) / sizeof(int);
         var headerOffsets = Converters.ByteArrayToInt32Array(_headerData.Skip(position).ToArray(), offsetLength);

         return new MapHeader
         {
             RLEWTag = rlewTag,
             NumPlanes = DefaultNumberOfPlanes,
             HeaderOffsets = headerOffsets,
             NumAvailableMaps = headerOffsets.Length,
             NumMaps = headerOffsets.Count(x => x > 0)
         };
    }

    public List<MapHeaderSegment> GetMapHeaderSegmentsList()
    {
        var mapHeader = GetHeader();
        var segments = new List<MapHeaderSegment>();
        var position = 0;
        
        foreach (var offset in mapHeader.HeaderOffsets)
        {
            // Sparse map
            if (offset == 0) continue;
            
            position = offset;
            var planeStarts = Converters.ByteArrayToInt32Array(
                _gameMapData.Skip(position).Take(sizeof(int) * DefaultNumberOfPlanes).ToArray(), DefaultNumberOfPlanes);
            position += sizeof(int) * DefaultNumberOfPlanes;
            
            var planeLengths = Converters.ByteArrayToUInt16Array(
                _gameMapData.Skip(position).Take(sizeof(ushort) * DefaultNumberOfPlanes).ToArray(), DefaultNumberOfPlanes);
            position += sizeof(ushort) * DefaultNumberOfPlanes;

            var width = BitConverter.ToUInt16(_gameMapData.Skip(position).ToArray());
            position += sizeof(ushort);
            
            var height = BitConverter.ToUInt16(_gameMapData.Skip(position).ToArray());
            position += sizeof(ushort);

            var nameBytes = _gameMapData.Skip(position)
                .Take(sizeof(byte) * DefaultMapNameSize).ToArray();
            // Find the string terminator \0
            var nameEndOfString = Array.FindIndex(nameBytes, x => x == 0x00);
            var name = System.Text.Encoding.UTF8.GetString(nameBytes.Take(nameEndOfString).ToArray());
            segments.Add(new MapHeaderSegment
            {
                PlaneStarts = planeStarts,
                PlaneLengths = planeLengths,
                Height = height,
                Width = width,
                Name = name
            });
        }

        return segments;
    }

    public override List<Asset> Load()
    {
        var assets = new List<Asset>();
        var segments = GetMapHeaderSegmentsList();
        foreach (var segment in segments)
        {
            var mapAsset = new MapAsset
            {
                Name = segment.Name,
                NumPlanes = DefaultNumberOfPlanes,
                Width = segment.Width,
                Height = segment.Height,
                AssetType = AssetType.Map,
                PlaneData = new ushort[DefaultNumberOfPlanes][]
            };
            for (var plane = 0; plane < DefaultNumberOfPlanes; plane++)
            {
                var position = segment.PlaneStarts[plane];
                var compressedSize = segment.PlaneLengths[plane];
                if (compressedSize == 0)
                    continue;

                var compressedData = _gameMapData.Skip(position).Take(compressedSize).ToArray();
                var carmackCompression = new CarmackCompression(/*nearTag, farTag*/);

                //
                // unhuffman, then unRLEW
                // The huffman'd chunk has a two byte expanded length first
                // The resulting RLEW chunk also does, even though it's not really
                // needed
                //
                var expanded = BitConverter.ToUInt16(compressedData.Take(sizeof(ushort)).ToArray());
                var expandedData = carmackCompression.Expand(compressedData.Skip(sizeof(ushort)).ToArray()).Take(expanded).ToArray();
                
                var rlewCompression = new RLEWCompression(/*rlewTag*/);
                var size = expandedData.First();
                var mapPlaneData = rlewCompression.Expand(Converters.UInt16ArrayToByteArray(expandedData.Skip(1).ToArray())).Take(size).ToArray();
                mapAsset.PlaneData[plane] = mapPlaneData;
            }

            assets.Add(mapAsset);
        }
        return assets;
    }

    public record MapHeader
    {
        public ushort RLEWTag { get; init; }
        public ushort NumPlanes { get; init; } = 3;
        public int[] HeaderOffsets { get; init; } = [];

        public int NumAvailableMaps { get; set; }
        public int NumMaps { get; set; }
    }

    public record MapHeaderSegment
    {
        public int[] PlaneStarts { get; set; } = new int[DefaultNumberOfPlanes];
        public ushort[] PlaneLengths { get; set; } = new ushort[DefaultNumberOfPlanes];
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}