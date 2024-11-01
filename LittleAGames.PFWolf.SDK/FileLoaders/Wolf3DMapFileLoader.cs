using LittleAGames.PFWolf.SDK.Abstract;
using LittleAGames.PFWolf.SDK.Assets;
using LittleAGames.PFWolf.SDK.Utilities;

namespace LittleAGames.PFWolf.SDK.FileLoaders;

public class Wolf3DMapFileLoader : BaseFileLoader
{
    private readonly byte[] _headerData;
    private readonly byte[] _gameMapData;

    public Wolf3DMapFileLoader(string directory, string mapHead, string gameMap) : base(directory)
    {
        var mapHeaderFilePath = Path.Combine(Directory, mapHead);
        _headerData = File.ReadAllBytes(mapHeaderFilePath);

        var gameMapsFilePath = Path.Combine(Directory, gameMap);
        _gameMapData = File.ReadAllBytes(gameMapsFilePath);
    }

    public MapHeader GetHeaderList()
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
             NumPlanes = 3,
             HeaderOffsets = headerOffsets
         };
    }

    public override List<Asset> Load()
    {
        var headers = GetHeaderList();
        return [];
    }

    public class MapHeader
    {
        public ushort RLEWTag { get; set; }
        public ushort NumPlanes { get; set; } = 3;
        public int[] HeaderOffsets { get; set; } = [];
    }
}