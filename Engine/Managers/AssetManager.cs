using LittleAGames.PFWolf.Common.Models;
using LittleAGames.PFWolf.Common.Utilities;
using LittleAGames.PFWolf.FileManager;
using LittleAGames.PFWolf.SDK;

namespace Engine.Managers;

public class AssetManager
{
    private readonly FileLoader _fileLoader;
    private readonly List<Asset> _assets = new List<Asset>();

    public AssetManager(FileLoader fileLoader)
    {
        _fileLoader = fileLoader;
    }
    
    // TODO: This will take in a string "asset" and return the metadata, and byte data for that asset
    public void AddGamePack(GamePack gamePack, string directory)
    {
        var loadedGamePack = _fileLoader.Load(gamePack, directory);
        
        // Audio stuff
        loadedGamePack.TryGetValue("AUDIOHED.WL6", out var audioHedData); 
        loadedGamePack.TryGetValue("AUDIOT.WL6", out var audioTData);
        var audioAssets = new Wolf3DAudioAssetLoader(audioHedData, audioTData).Get();
        _assets.AddRange(audioAssets);
        
        loadedGamePack.TryGetValue("maphead.wl6", out var mapHeadData); 
        loadedGamePack.TryGetValue("gamemaps.wl6", out var gameMapsData);
        //var mapAssets = new Wolf3DMapAssetLoader(mapHeadData, gameMapsData).Get();
        //_assets.AddRange(mapAssets);

        loadedGamePack.TryGetValue("vswap.wl6", out var vswapData); 
        //var vSwapAssets = new Wolf3DVswapAssetLoader(vSwapData).Get();
        //_assets.AddRange(vSwapAssets);
        
        loadedGamePack.TryGetValue("vgadict.wl6", out var vgaDictData); 
        loadedGamePack.TryGetValue("vgahead.wl6", out var vgaHeadData); 
        loadedGamePack.TryGetValue("vgagraph.wl6", out var vgaGraphData);
        //var vgaAssets = new Wolf3DVgaDataAssetLoader(vgaDictData, vgaHeadData, vgaGraphData).Get();
        //_assets.AddRange(vgaAssets);

    }

    public void AddModPack(object pk3File)
    {
        throw new NotImplementedException();
    }
}

public abstract class AssetLoader
{
    public abstract List<Asset> Get();
}

public class Wolf3DAudioAssetLoader : AssetLoader
{
    private readonly byte[] _audioHedData;
    private readonly byte[] _audioTData;

    public Wolf3DAudioAssetLoader(byte[] audioHedData, byte[] audioTData)
    {
        _audioHedData = audioHedData;
        _audioTData = audioTData;
    }
    
    public override List<Asset> Get()
    {

        var audioStarts = Converters.ByteArrayToInt32Array(_audioHedData, length: _audioHedData.Length / sizeof(int));

        var numLumps = audioStarts.Length - 1;

        var audioHeaderData = new List<AudioHeaderData>(numLumps);

        for (var index = 0; index < numLumps; index++)
        {
            audioHeaderData.Add(new AudioHeaderData
            {
                Index = index,
                DataFilePosition = audioStarts[index]
            });
        }

        var audioAssets = new List<Asset>(audioHeaderData.Count);

        var endOfSoundsIndex = -1;
        var musicStartIndex = -1;

        foreach (var header in audioHeaderData)
        {
            var pos = header.DataFilePosition;
            var nextHeaderPosition = audioHeaderData.FirstOrDefault(h => h.Index == header.Index + 1)?.DataFilePosition ?? _audioTData.Length;
            var chunkLength = nextHeaderPosition - header.DataFilePosition;

            // Find the gap of "sparse" records between AdLib sounds and Music (if applicable)
            if (chunkLength <= 0)
            {
                if (endOfSoundsIndex == -1)
                    endOfSoundsIndex = header.Index - 1;

                continue;
            }

            // Find the music start marker
            if (chunkLength == 4)
            {
                if (musicStartIndex == -1)
                    musicStartIndex = header.Index + 1;
                continue;
            }

            audioAssets.Add(new AudioAsset
                {
                    Name = $"Audio Asset #{header.Index}",
                    // TODO: Might be able to just get the position, and size for each asset for lazy loading
                    RawData = _audioTData.Skip(pos).Take(chunkLength).ToArray()
                });
        }

        // Should be 50/50 of PC to AdLib sounds
        var soundTotal = endOfSoundsIndex / 2;

        // If the 4-byte marker doesn't exist, just start after the sound chunks
        if (musicStartIndex <= 0)
            musicStartIndex = endOfSoundsIndex;

        // TODO: If the marker exists for music, but there are no sparse sound records, then what?
        // Use the marker first?

        // Categorize audio data
        foreach (var audio in audioAssets.Where(x => audioAssets.IndexOf(x) <= soundTotal))
        {
            audio.AssetType = AssetType.PcSound;
        }
        
        foreach (var audio in audioAssets.Where(x => audioAssets.IndexOf(x) > soundTotal && audioAssets.IndexOf(x) <= endOfSoundsIndex))
        {
            audio.AssetType = AssetType.AdLibSound;
        }
        
        foreach (var audio in audioAssets.Where(x => audioAssets.IndexOf(x) >= musicStartIndex))
        {
            audio.AssetType = AssetType.ImfMusic;
        }

        return audioAssets;
    }
}

public enum AssetType
{
    Unknown,
    PcSound,
    AdLibSound,
    ImfMusic,
}

public abstract class Asset
{
    public abstract string Name { get; set; }
    public abstract byte[] RawData { get; set; }
    public AssetType AssetType { get; set; } = AssetType.Unknown;
}

public class AudioAsset : Asset
{
    public override string Name { get; set; }
    public override byte[] RawData { get;  set; }
}


public class PcSoundAsset : AudioAsset
{
}

public class AdLibSoundAsset : AudioAsset
{
}

public class ImfMusicAsset : AudioAsset
{
}