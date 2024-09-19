using LittleAGames.PFWolf.Common.Utilities;
using System.Collections.ObjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LittleAGames.PFWolf.Common.Models;

public class SwapData
{

    /// <summary>
    /// Contains entirety of the header and page data information in the VSWAP file
    /// </summary>
    private readonly byte[] _data = null!;

    public ushort ChunksInFile { get; private set; }
    public ushort SpriteStartIndex { get; private set; }
    public ushort SoundStartIndex { get; private set; }

    public ReadOnlyCollection<uint> PageOffsets { get; private set; }
    public ReadOnlyCollection<ushort> PageLengths { get; private set; }

    public SwapData(byte[] data)
    {
        _data = data;

        var position = 0;

        ChunksInFile = BitConverter.ToUInt16(_data, position);
        position += sizeof(UInt16);

        SpriteStartIndex = BitConverter.ToUInt16(_data, position);
        position += sizeof(UInt16);

        SoundStartIndex = BitConverter.ToUInt16(_data, position);
        position += sizeof(UInt16);

        var bytesOfPageOffsets = _data.Skip(position).Take(sizeof(UInt32) * ChunksInFile);
        var pageOffsets = Converters.ByteArrayToUInt32Array(bytesOfPageOffsets.ToArray(), ChunksInFile + 1);
        position += sizeof(UInt32) * ChunksInFile;

        var bytesOfPageLengths = _data.Skip(position).Take(sizeof(UInt16) * ChunksInFile);
        PageLengths = new (Converters.ByteArrayToUInt16Array(bytesOfPageLengths.ToArray(), ChunksInFile));
        position += sizeof(UInt16) * ChunksInFile;

        var dataSize = (int)(_data.Length - pageOffsets.First());

        if (dataSize < 0)
        {
            // Why does it say "too large?"
            throw new Exception("Data size too large");
        }
        
        // Used for the last chunk size
        pageOffsets[ChunksInFile] = (uint)_data.Length;
        PageOffsets = new(pageOffsets);

        // If the size of data is larger than whats left, what happens
        //var pageData = data.Skip(position).Take(dataSize).ToArray();
        //position += dataSize;

        // There's a little bit of data at the end of the file, is this used?
        //var whatsthis = _data.Skip(position).ToArray(); // last bytes of data
        // TODO: The data part

        for (var i = 0; i < ChunksInFile; i++)
        {
            if (pageOffsets[i] == 0)
                continue;           // sparse page

            if (pageOffsets[i] < pageOffsets[0] || pageOffsets[i] >= _data.Length)
            {
                throw new Exception($"Illegal page offset for page {i}: {pageOffsets[i]} (filesize: {_data.Length})");
            }
        }
    }

    public string Display()
        => $"Chunks: {ChunksInFile}, Sprite Start: {SpriteStartIndex}, Sound Start: {SoundStartIndex}";

    public List<SwapEntry> GetEntries()
    {
        var entries = new List<SwapEntry>();
        var i = 0;
        for (i = 0; i < SpriteStartIndex; i++)
        {
            if (PageOffsets[i] == 0)
            {
                // "sparse" page
                entries.Add(new SparseEntry(i));
                continue;
            }

            var data = _data.Skip((int)PageOffsets[i]).Take(PageLengths[i]).ToArray();
            entries.Add(new WallEntry(data, i));
        }

        for (i = SpriteStartIndex; i < SoundStartIndex; i++)
        {
            if (PageOffsets[i] == 0)
            {
                // "sparse" page
                entries.Add(new SparseEntry(i));
                continue;
            }

            var data = _data.Skip((int)PageOffsets[i]).Take(PageLengths[i]).ToArray();
            entries.Add(new SpriteEntry(data, i));
        }

        // Sounds stretch over multiple pages
        var soundPages = new List<byte[]>();
        for (i = SoundStartIndex; i < ChunksInFile; i++)
        {
            soundPages.Add(_data.Skip((int)PageOffsets[i]).Take(PageLengths[i]).ToArray());
        }

        var soundInfoPage = soundPages.Last();
        ushort[] soundInfoData = Converters.ByteArrayToUInt16Array(soundInfoPage, soundInfoPage.Length / sizeof(ushort));
        var numDigiSounds = soundInfoPage.Length / 4;
        //var digiSoundEntries = new List<DigitizedSoundEntry>(numDigiSounds);

        // Join pages to complete the sound files
        for (var pIndex = 0; pIndex < numDigiSounds; pIndex++)
        {
            var startPage = soundInfoData[pIndex * 2];
            if (startPage >= ChunksInFile - 1)
            {
                numDigiSounds = pIndex;
                break;
            }

            int lastPage;
            if (pIndex < numDigiSounds - 1)
            {
                lastPage = soundInfoData[pIndex * 2 + 2];
                if (lastPage == 0 || lastPage + SoundStartIndex > ChunksInFile - 1)
                    lastPage = ChunksInFile - 1;
                else
                    lastPage += SoundStartIndex;
            }
            else
                lastPage = ChunksInFile - 1;

            //long size = 0;
            int page;
            var soundData = new List<byte>();
            for(i = startPage, page = SoundStartIndex + startPage; page < lastPage; page++, i++)
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
            entries.Add(new DigitizedSoundEntry(soundData.ToArray(), entries.Count));
            //DigiList[i].length = size;

            // TODO: Add all of the pages to the "data" of the digisoundentry

        }

        return entries;
    }
}
