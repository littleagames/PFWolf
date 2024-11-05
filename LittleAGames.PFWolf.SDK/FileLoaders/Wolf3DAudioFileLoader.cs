using LittleAGames.PFWolf.SDK.Abstract;
using LittleAGames.PFWolf.SDK.Assets;
using LittleAGames.PFWolf.SDK.Utilities;

namespace LittleAGames.PFWolf.SDK.FileLoaders;

public class Wolf3DAudioFileLoader : BaseFileLoader
{
    private readonly List<string> _assetReferences;
    private readonly byte[] _headerData;
    private readonly byte[] _audioData;

    public Wolf3DAudioFileLoader(string directory, List<string> assetReferences, string audioHed, string audioT)
        : base(directory)
    {
        _assetReferences = assetReferences;
        var audioHeaderFilePath = Path.Combine(Directory, audioHed);
        _headerData = File.ReadAllBytes(audioHeaderFilePath);

        var audioDataFilePath = Path.Combine(Directory, audioT);
        _audioData = File.ReadAllBytes(audioDataFilePath);
    }

    public override List<Asset> Load()
    {
        var headerList = GetAudioHeaderList();
        var audioDataList = new List<Asset>(headerList.Count);

        for (var i = 0; i < headerList.Count; i++)
        {
            var header = headerList[i];
            var pos = header.DataFilePosition;
            var chunkLength = header.Size;

            // Find the music start marker
            if (chunkLength == 4)
            { 
                // audioDataList.Add(new AudioMarkerAsset
                // {
                //     Name = $"Marker",
                //     AssetType = AssetType.AudioMarker,
                //     RawData = _audioData.Skip(pos).Take(chunkLength).ToArray()
                // });
                continue;
            }
            
            audioDataList.Add(new AudioAsset
            {
                Name = $"AUD{i:D5}",
                AssetType = header.Namespace,
                RawData = _audioData.Skip(pos).Take(chunkLength).ToArray()
            });
        }

        return audioDataList;
    }

    public List<AudioHeaderData> GetAudioHeaderList()
    {
        uint[] segmentStarts = [0,0,0,0];
        uint currentSegment = 0;
        var numLumps = (_headerData.Length / 4)-1;
        
        var audioStarts = Converters.ByteArrayToInt32Array(_headerData, length: _headerData.Length / sizeof(uint));

        var audioHeaders = new List<AudioHeaderData>(numLumps);

        var segmentAssetType = new[]{ AssetType.PcSound, AssetType.AdLibSound, AssetType.DigiSound, AssetType.ImfMusic };
        for (var index = 0; index < numLumps; index++)
        {
            int size = audioStarts[index + 1] - audioStarts[index];
            audioHeaders.Add(new AudioHeaderData
            {
                Index = index,
                DataFilePosition = audioStarts[index],
                Size = size,
                Namespace = segmentAssetType[currentSegment]
            });
            
            // There are 4 segments in the audiot file (pc, adlib, digi, music)
            // and they are all separated by a 4 byte marker "!ID!"
            
            // Try to find !ID! tags
            if(currentSegment < 3 && size >= 4)
                // Okay, size < 4, that's either 0 empty, or 4, a marker
                // What files have the !ID! tag? (This is also ASCII)
            {
                var tagBytes = _audioData.Skip(audioStarts[index] + size - 4).Take(4).ToArray();
                var tag = System.Text.Encoding.UTF8.GetString(tagBytes);
                if (tag.Equals(Asset.AssetMarker))
                {
                    // TODO: Add "marker" to assets
                    segmentStarts[++currentSegment] = (uint)index+1;
                    // Lump end contains a !ID! tag, remove it from the lump
                    audioHeaders[index].Size -= 4;
                }
            }
        }
        
        // If after checking all of the segments, and there were not 4,
        // Find the audio entry that is 0 to 4 bytes in length
        // Most wolf3d files don't have digi sounds stored, so they'll be 0
        // and there
        if(currentSegment != 3)
        {
            for(int i = numLumps-(numLumps%3)-1;i >= 0;i -= 3)
            {
                if(audioHeaders[i].Size <= 4)
                {
                    segmentStarts[3] = (uint)++i;
                    for(;i < numLumps;++i)
                        audioHeaders[i].Namespace = AssetType.ImfMusic;
                    break;
                }
            }
        }

        return audioHeaders;
    }
}

public class AudioHeaderData
{
    public int Index { get; init; }
    public string Name => $"Audio Header {Index} (Pos: {DataFilePosition}), NS:{Namespace}";

    /// <summary>
    /// Position in data file where chunk is stored
    /// </summary>
    public int DataFilePosition { get; init; }

    public int Size { get; set; }
    public AssetType Namespace { get; set; } = AssetType.Unknown;
}