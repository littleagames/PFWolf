// using LittleAGames.PFWolf.Common.Models;
//
// namespace LittleAGames.PFWolf.FileManager;
//
// public class AudioFileManager
// {
//     private readonly string audioFilePath;
//     private readonly string audioHeaderFilePath;
//
//     public AudioFileManager(string audioFilePath, string audioHeaderFilePath)
//     {
//         this.audioFilePath = audioFilePath;
//         this.audioHeaderFilePath = audioHeaderFilePath;
//     }
//
//     public List<AudioHeaderData> GetAudioHeaderList()
//     {
//         var headerFile = File.ReadAllBytes(audioHeaderFilePath);
//
//         var audioStarts = Converters.ByteArrayToInt32Array(headerFile, length: headerFile.Length / sizeof(int));
//
//         var numLumps = audioStarts.Length - 1;
//
//         var audioHeaders = new List<AudioHeaderData>(numLumps);
//
//         for (var index = 0; index < numLumps; index++)
//         {
//             audioHeaders.Add(new AudioHeaderData
//             {
//                 Index = index,
//                 DataFilePosition = audioStarts[index]
//             });
//         }
//
//         return audioHeaders;
//     }
//
//     /// <summary>
//     /// Audio data should be laid out in 3 sections:
//     /// 1) PC Sounds
//     /// 2) AdLib Sounds
//     /// 3) Music
//     /// 
//     /// Music should be divided by a marker, and/or a gap of "sparse" entries
//     /// PC/AdLib sounds should be always an even amount, as there should always be a match between the two
//     /// but this possibly may not happen. TBD to determine what each byte[] stores
//     /// </summary>
//     /// <returns></returns>
//     public List<AudioData> GetAudioList()
//     {
//         var headerList = GetAudioHeaderList();
//         var data = File.ReadAllBytes(audioFilePath);
//
//         var audioDataList = new List<AudioData>(headerList.Count);
//
//         var endOfSoundsIndex = -1;
//         var musicStartIndex = -1;
//
//         foreach (var header in headerList)
//         {
//             var pos = header.DataFilePosition;
//             var nextHeaderPosition = headerList.FirstOrDefault(h => h.Index == header.Index + 1)?.DataFilePosition ?? data.Length;
//             var chunkLength = nextHeaderPosition - header.DataFilePosition;
//
//             // Find the gap of "sparse" records between AdLib sounds and Music (if applicable)
//             if (chunkLength <= 0)
//             {
//                 if (endOfSoundsIndex == -1)
//                     endOfSoundsIndex = header.Index - 1;
//
//                 continue;
//             }
//
//             // Find the music start marker
//             if (chunkLength == 4)
//             {
//                 if (musicStartIndex == -1)
//                     musicStartIndex = header.Index + 1;
//                 continue;
//             }
//
//             audioDataList.Add(new AudioData
//             {
//                 Index = header.Index,
//                 Data = data.Skip(pos).Take(chunkLength).ToArray()
//             });
//         }
//
//         // Should be 50/50 of PC to AdLib sounds
//         var soundTotal = endOfSoundsIndex / 2;
//
//         // If the 4-byte marker doesn't exist, just start after the sound chunks
//         if (musicStartIndex <= 0)
//             musicStartIndex = endOfSoundsIndex;
//
//         // TODO: If the marker exists for music, but there are no sparse sound records, then what?
//         // Use the marker first?
//
//         // Categorize audio data
//         foreach (var audio in audioDataList.Where(x => x.Index <= soundTotal))
//         {
//             audio.AudioType = AudioType.PcSound;
//         }
//
//         foreach (var audio in audioDataList.Where(x => x.Index > soundTotal && x.Index <= endOfSoundsIndex))
//         {
//             audio.AudioType = AudioType.AdLibSound;
//         }
//
//         foreach (var audio in audioDataList.Where(x => x.Index >= musicStartIndex))
//         {
//             audio.AudioType = AudioType.Music;
//         }
//
//         return audioDataList;
//     }
// }
//
// //public class AudioData
// //{
// //    public List<PcSound> PCSounds { get; set; } = [];
// //    public List<AdLibSound> AdLibSounds { get; set; } = [];
// //    public List<Music> Music { get; set; } = [];
//
// //    private Dictionary<int, byte[]> Data
// //    {
// //        get
// //        {
// //            return PCSounds.ToDictionary(x => x.Index, x => x.Data)
// //                .Concat(AdLibSounds.ToDictionary(x => x.Index, x => x.Data))
// //                .Concat(Music.ToDictionary(x => x.Index, x => x.Data))
// //                .ToDictionary(x => x.Key, x => x.Value);
// //        }
// //    }
//
// //    public bool IsValidValue(int value)
// //    {
// //        return PCSounds.Select(x => x.Index).Any(x => x == value)
// //            || AdLibSounds.Select(x => x.Index).Any(x => x == value)
// //            || Music.Select(x => x.Index).Any(x => x == value);
// //    }
//
//
//
// //    public Result PlayAudio(int value)
// //    {
// //        // PC Speaker
// //        // First 6 bytes is data
// //            // [0-3] - size of sample
// //            // [4-5] - priority
// //            // [6+size] - data
//
// //        if (!Data.TryGetValue(value, out var audio))
// //        {
// //            return Result.Failure($"Audio {value} does not exist.");
// //        }
//
// //        var len = BitConverter.ToInt32(audio.Take(4).ToArray());
// //        var priority = BitConverter.ToInt16(audio.Skip(4).Take(2).ToArray());
//
// //        Console.WriteLine($"Size: {len}");
// //        Console.WriteLine($"Priority: {priority}");
//
//
// //        // PC Speaker sample rate is 140Hz
//
// //        // The PC PIC counts down at 1.193180MHz
// //        // So pwm_freq = counter_freq / reload_value
// //        // reload_value = pcLastSample * 60 (see SDL_DoFX)
// //        //current_freq = 1193180 / (pcLastSample * 60);
// //        for (var i = 6; i < len+6; i++) {
// //            Console.Write($"[{audio[i]}]");
// //        }
//
// //        Console.WriteLine();
//
// //        // TODO: Run an audio manager
// //        // Audio manager will take the byte data and convert it to a PC sound
// //        // PC Sound will play???
//
// //        //Type SDL_PCMixCallback = null;
// //        //SDL2.SDL.SDL_AudioCallback.CreateDelegate(p => SDL_PCMixCallback, null);
// //        //Mix_SetPostMix(SDL_PCMixCallback, NULL);
// //        return Result.Success();
// //    }
// //}
//
// //public abstract class AudioDataSegment
// //{
// //}
//
// //public class PcSound : AudioDataSegment
// //{
// //    public int Index { get; set; }
// //    public string Name { get => $"PC Sound {Index:000} (Size: {Data.Length} bytes)"; }
// //    public byte[] Data { get; set; } = [];
//
// //    public int DataSize { get; set; } // first 4 bytes of data
// //    public short Priority { get; set; } // next 2 bytes of data
// //    public byte[] AudioData { get; set; } // remaining from 6 to "DataSize"
// //}
//
// //public class AdLibSound
// //{
// //    public int Index { get; set; }
// //    public string Name { get => $"AdLib Sound {Index:000} (Size: {Data.Length} bytes)"; }
// //    public byte[] Data { get; set; } = [];
// //}
//
// //public class Music
// //{
// //    public int Index { get; set; }
// //    public string Name { get => $"Music {Index:000} (Size: {Data.Length} bytes)"; }
// //    public byte[] Data { get; set; } = [];
// //}
//
// //internal class AudioFileManager : DataAndHeaderFileManager<AudioData>
// //{
// //    public AudioFileManager(string dataFile, string headerFile) : base(dataFile, headerFile)
// //    {
//
// //    }
//
// //    public override Result<AudioData> Load()
// //    {
// //        var audioStarts = Converters.ByteArrayToInt32Array(_header, length: _header.Length / sizeof(Int32));
//
// //        var numLumps = audioStarts.Length - 1;
//
// //        var audioSegments = new List<byte[]>();
//
// //        for(var i = 0; i < numLumps; i++)
// //        {
// //            var pos = audioStarts[i];
// //            var segLength = 0;
// //            if (i < audioStarts.Length - 1)
// //            {
// //                segLength = audioStarts[i+1] - pos;
// //            }
// //            else
// //            {
// //                segLength = _data.Length;
// //            }
//
// //            audioSegments.Add(_data.Skip(pos).Take(segLength).ToArray());
// //        }
//
// //        /*
// //         PC sounds and AdLib sounds take up the same amount of slots
// //        If there aren't any sounds available, the slot may take up a "start seg" that
// //        Is only 4 bytes (which would play a "dead" sound)
// //         */
//
// //        // Don't always rely on it, but the pc/adlib sounds might not have all, and they are 0
// //        // Why is 289 a 0 byte?
// //        // TODO: What does ECWolf do when it looks for these files?
//
// //        // TODO: There is a 4 byte chunk that is a space between pc sounds, and music
// //        var musicMarker = audioSegments.First(x => x.Length == 4);
// //        var indexOfMusic = audioSegments.IndexOf(musicMarker) + 1;
//
// //        var indexOfLastAdLib = 0;
// //        var possibleAdLibMarker = audioSegments.FirstOrDefault(x => x.Length == 0);
// //        if (possibleAdLibMarker != null)
// //        {
// //            indexOfLastAdLib = audioSegments.IndexOf(possibleAdLibMarker);
// //        }
// //        else
// //        {
// //            indexOfLastAdLib = indexOfMusic - 1;
// //        }
//
// //        var numSounds = indexOfLastAdLib / 2;
//
// //        //for (var s = 0; s < audioSegments.Count; s++)
// //        //{
// //           // var seg = audioSegments[s];
// //            //var pos = 0;
// //            // Take a 4 byte array, convert it to chars
// //            // Compare the chars to "!ID!"
// //            // Discard all previous in the data chunk?
// //            //for (pos = 0; pos < seg.Length - 4; pos += 4)
// //            //{
// //            //var word = new string(seg.Select(x => (char)x).ToArray());
// //            //var index = (word.IndexOf("!ID!", StringComparison.InvariantCultureIgnoreCase));
// //            //if (index > -1)
// //            //{
// //            //    // pos -= 4;
// //            //    // seg = seg.Skip(pos).ToArray();
// //            //    break;
// //            //}
// //           // }
// ////        }
//
// //        return new AudioData
// //        {
// //            PCSounds = audioSegments.Take(numSounds).Select(x => new PcSound { Index = audioSegments.IndexOf(x), Data = x }).ToList(),
// //            AdLibSounds = audioSegments.Skip(numSounds).Take(numSounds).Select(x => new AdLibSound { Index = audioSegments.IndexOf(x), Data = x }).ToList(),
// //            Music = audioSegments.Skip(indexOfMusic).Select(x => new Music { Index = audioSegments.IndexOf(x), Data = x }).ToList()
// //        };
// //    }
// //}