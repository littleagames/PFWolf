using Engine.Managers.AssetLoaders.Models;
using LittleAGames.PFWolf.Common.Models;
using LittleAGames.PFWolf.Common.Utilities;

namespace Engine.Managers.AssetLoaders;

public class Wolf3DMapAssetLoader : AssetLoader
{
    private readonly byte[] _mapHeaderData;
    private readonly byte[] _gameMapsData;

    public Wolf3DMapAssetLoader(byte[] mapHeaderData, byte[] gameMapsData)
    {
        _mapHeaderData = mapHeaderData;
        _gameMapsData = gameMapsData;
    }
    public override List<Asset> Get()
    {
        var position = 0;

        // TODO: What does "RLEW" mean?
        var rlewTag = BitConverter.ToUInt16(_mapHeaderData, 0);
        position += sizeof(ushort);

        // Check header length to end of file (if it's divisible by 4, then there is no numplanes tag)
        var hasNumPlanesBlock = (_mapHeaderData.Length - position) / 2 % 2 != 0;
        var numPlanes = 3;
        if (hasNumPlanesBlock)
        {
            // DOS didn't have the "numplanes" stored on the map here
            numPlanes = BitConverter.ToUInt16(_mapHeaderData, sizeof(ushort));
            if (numPlanes < 15)
            {
                position += sizeof(ushort);
            }
            else
            {
                numPlanes = 3; // Default in most wolf map files
            }
        }

        // Original map seems to have 100 maps placed
        var numMapSlots = (_mapHeaderData.Length - position) / sizeof(int);

        var headerOffsets = Converters.ByteArrayToInt32Array(_mapHeaderData.Skip(position).ToArray(), length: numMapSlots);

        var headerData = new List<MapHeaderData>(headerOffsets.Length);

        for (var index = 0; index < headerOffsets.Length; index++)
        {
            headerData.Add(new MapHeaderData
            {
                Index = index,
                MapHeaderSegmentPosition = headerOffsets[index]
            });
        }
        
        // Some times the map header is padded with extra values,
        // Drop them, and re-calculate the count of maps
        var numMaps = headerData.Count(x => x.MapHeaderSegmentPosition > 0);

        var maps = new List<Asset>(numMaps);

        // The last map contains the map metadata at the end of the file, so this should apply to all maps
        // as the metadata is the same size for each, we can safely determine numPlanes, and this can determine
        // the size of the map's name as well (usually 16 single-byte chars, or 32 single-byte chars via WDC)
        var mapMetadataSize = _gameMapsData.Length - headerData.Last(x => x.MapHeaderSegmentPosition > 0).MapHeaderSegmentPosition;

        for (var i = 0; i < numMaps; i++)
        {
            position = headerData[i].MapHeaderSegmentPosition;
            var nextHeaderPosition = i < numMaps - 1 ? headerData[i + 1].MapHeaderSegmentPosition : _gameMapsData.Length;
            var chunkLength = nextHeaderPosition - position;

            var rawMetadata = _gameMapsData.Skip(position).Take(mapMetadataSize).ToArray();
            maps.Add(new MapAsset
            {
                Name = $"Map #{i}",
                RawData = rawMetadata, // TODO: This isn't all of it, is it?
                // Index = headerData[i].Index,
                // MapMetadata = rawMetadata,
                // HeaderMetadata = MapHeaderMetadata.Build(rawMetadata, numPlanes)
            });
        }

        return maps;
    }
}