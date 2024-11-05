using System.Collections.ObjectModel;
using LittleAGames.PFWolf.SDK.Abstract;
using LittleAGames.PFWolf.SDK.Assets;
using LittleAGames.PFWolf.SDK.Utilities;

namespace LittleAGames.PFWolf.SDK.FileLoaders;

public class Wolf3DVswapFileLoader : BaseFileLoader
{
    private readonly List<string> _assetReferences;
    private readonly byte[] _vswapData;

    public Wolf3DVswapFileLoader(string directory, List<string> assetReferences, string vswap) : base(directory)
    {
        _assetReferences = assetReferences;
        var vswapFilePath = Path.Combine(Directory, vswap);
        _vswapData = File.ReadAllBytes(vswapFilePath);
    }

    public SwapHeaderInfo GetHeaderInfo()
    {
        var position = 0;

        var chunksInFile = BitConverter.ToUInt16(_vswapData, position);
        position += sizeof(UInt16);

        var spriteStartIndex = BitConverter.ToUInt16(_vswapData, position);
        position += sizeof(UInt16);

        var soundStartIndex = BitConverter.ToUInt16(_vswapData, position);
        position += sizeof(UInt16);
        
        var bytesOfPageOffsets = _vswapData.Skip(position).Take(sizeof(UInt32) * chunksInFile);
        var pageOffsets = Converters.ByteArrayToUInt32Array(bytesOfPageOffsets.ToArray(), chunksInFile + 1);
        position += sizeof(UInt32) * chunksInFile;

        var bytesOfPageLengths = _vswapData.Skip(position).Take(sizeof(UInt16) * chunksInFile);
        var pageLengths = Converters.ByteArrayToUInt16Array(bytesOfPageLengths.ToArray(), chunksInFile);
        position += sizeof(UInt16) * chunksInFile;

        var dataSize = (int)(_vswapData.Length - pageOffsets.First());

        if (dataSize < 0)
        {
            // Why does it say "too large?"
            throw new Exception("Data size too large");
        }
        
        // Used for the last chunk size
        pageOffsets[chunksInFile] = (uint)_vswapData.Length;
        
        // If the size of data is larger than whats left, what happens
        //var pageData = data.Skip(position).Take(dataSize).ToArray();
        //position += dataSize;

        // There's a little bit of data at the end of the file, is this used?
        //var whatsthis = _data.Skip(position).ToArray(); // last bytes of data
        // TODO: The data part

        for (var i = 0; i < chunksInFile; i++)
        {
            if (pageOffsets[i] == 0)
                continue;           // sparse page

            if (pageOffsets[i] < pageOffsets[0] || pageOffsets[i] >= _vswapData.Length)
            {
                throw new Exception($"Illegal page offset for page {i}: {pageOffsets[i]} (filesize: {_vswapData.Length})");
            }
        }

        return new SwapHeaderInfo
        {
            PageOffsets = pageOffsets,
            PageLengths = pageLengths,
            ChunksInFile = chunksInFile,
            SpriteStartIndex = spriteStartIndex,
            SoundStartIndex = soundStartIndex
        };
    }

    public override List<Asset> Load()
    {
        var headerInfo = GetHeaderInfo();
        var assets = new List<Asset>();
        var i = 0;
        for (i = 0; i < headerInfo.SpriteStartIndex; i++)
        {
            if (headerInfo.PageOffsets[i] == 0)
            {
                // "sparse" page
                //assets.Add(new SparseEntry(i));
                continue;
            }

            var data = _vswapData.Skip((int)headerInfo.PageOffsets[i]).Take(headerInfo.PageLengths[i]).ToArray();
            assets.Add(new WallAsset
            {
                Name = $"WALL{i:D5}",
                AssetType = AssetType.Texture,
                RawData = data
            });
        }

        for (i = headerInfo.SpriteStartIndex; i < headerInfo.SoundStartIndex; i++)
        {
            if (headerInfo.PageOffsets[i] == 0)
            {
                // "sparse" page
                //assets.Add(new SparseEntry(i));
                continue;
            }

            var data = _vswapData.Skip((int)headerInfo.PageOffsets[i]).Take(headerInfo.PageLengths[i]).ToArray();
            assets.Add(new SpriteAsset
            {
                Name = $"SPRITE{i:D5}",
                RawData = data
            });
        }

        // Sounds stretch over multiple pages
        var soundPages = new List<byte[]>();
        for (i = headerInfo.SoundStartIndex; i < headerInfo.ChunksInFile; i++)
        {
            soundPages.Add(_vswapData.Skip((int)headerInfo.PageOffsets[i]).Take(headerInfo.PageLengths[i]).ToArray());
        }

        var soundInfoPage = soundPages.Last();
        ushort[] soundInfoData = Converters.ByteArrayToUInt16Array(soundInfoPage, soundInfoPage.Length / sizeof(ushort));
        var numDigiSounds = soundInfoPage.Length / 4;
        //var digiSoundEntries = new List<DigitizedSoundEntry>(numDigiSounds);

        // Join pages to complete the sound files
        for (var pIndex = 0; pIndex < numDigiSounds; pIndex++)
        {
            var startPage = soundInfoData[pIndex * 2];
            if (startPage >= headerInfo.ChunksInFile - 1)
            {
                numDigiSounds = pIndex;
                break;
            }

            int lastPage;
            if (pIndex < numDigiSounds - 1)
            {
                lastPage = soundInfoData[pIndex * 2 + 2];
                if (lastPage == 0 || lastPage + headerInfo.SoundStartIndex > headerInfo.ChunksInFile - 1)
                    lastPage = headerInfo.ChunksInFile - 1;
                else
                    lastPage += headerInfo.SoundStartIndex;
            }
            else
                lastPage = headerInfo.ChunksInFile - 1;

            //long size = 0;
            int page;
            var soundData = new List<byte>();
            for(i = startPage, page = headerInfo.SoundStartIndex + startPage; page < lastPage; page++, i++)
            {
                soundData.AddRange(soundPages[i]);
                //size += soundPages[i].Length;
            }

            // TOOD: Padding thing

            //if ((size & 0xffff0000) != 0 && (size & 0xffff) < soundInfoPage[pIndex * 2 + 1])
            //{
            //    size -= 0x10000;
            //}

            //size = (size * 0xffff0000) | soundInfoPage[pIndex * 2 + 1];
            
            // assets.Add(new DigitizedSoundAsset
            // {
            //     Name = $"DIGI{i:D5}",
            //     RawData = soundData.ToArray()
            // });
            
            //DigiList[i].length = size;

            // TODO: Add all of the pages to the "data" of the digisoundentry

        }

        return assets;
    }
    
    public class SwapHeaderInfo
    {
        public ushort ChunksInFile { get; internal set; }
        public ushort SpriteStartIndex { get; internal set; }
        public ushort SoundStartIndex { get; internal set; }

        public uint[] PageOffsets { get; internal set; }
        public ushort[] PageLengths { get; internal set; }
    }
    
    public abstract class SwapEntry
    {
        public int Index { get; private set; }
        public byte[] Data { get; private set; } = [];
        public int Size { get => Data?.Length ?? 0; }

        public SwapEntry(byte[] data, int index)
        {
            Index = index;
            Data = data;
        }

        public abstract string Display();
    }

    public class SparseEntry : SwapEntry
    {
        public SparseEntry(int index) : base([], index)
        {
        }

        public override string Display()
            => $"Sparse Entry #{Index}";
    }

    public class WallEntry : SwapEntry
    {
        public WallEntry(byte[] data, int index)
            : base(data, index)
        {
        }

        public override string Display()
            => $"Wall #{Index} Size ({Size} bytes)";
    }

    public class SpriteEntry : SwapEntry
    {
        public SpriteEntry(byte[] data, int index)
            : base(data, index)
        {
        }

        public override string Display()
            => $"Sprite #{Index} Size ({Size} bytes)";
    }

    public class DigitizedSoundEntry : SwapEntry
    {
        public DigitizedSoundEntry(byte[] data, int index)
            : base(data, index)
        {
        }

        public override string Display()
            => $"Digi Sound #{Index} Size ({Size} bytes)";
    }
}