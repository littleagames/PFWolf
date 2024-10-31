// using System;
// using System.Linq;
// using System.Reflection.PortableExecutable;
// using System.Xml.Linq;
// using static System.Runtime.InteropServices.JavaScript.JSType;
//
// namespace LittleAGames.PFWolf.FileManager;
//
// public class MapFileManager
// {
//     private readonly string mapFilePath;
//     private readonly string mapHeaderFilePath;
//
//     public MapFileManager(string mapFilePath, string mapHeaderFilePath)
//     {
//         this.mapFilePath = mapFilePath;
//         this.mapHeaderFilePath = mapHeaderFilePath;
//     }
//
//     public MapHeaderList GetMapHeaderList()
//     {
//         var headerFile = File.ReadAllBytes(mapHeaderFilePath);
//
//         var position = 0;
//
//         // TODO: What does "RLEW" mean?
//         var rlewTag = BitConverter.ToUInt16(headerFile, 0);
//         position += sizeof(ushort);
//
//         // Check header length to end of file (if it's divisible by 4, then there is no numplanes tag)
//         var hasNumPlanesBlock = (headerFile.Length - position) / 2 % 2 != 0;
//         var numPlanes = 3;
//         if (hasNumPlanesBlock)
//         {
//             // DOS didn't have the "numplanes" stored on the map here
//             numPlanes = BitConverter.ToUInt16(headerFile, sizeof(ushort));
//             if (numPlanes < 15)
//             {
//                 position += sizeof(ushort);
//             }
//             else
//             {
//                 numPlanes = 3; // Default in most wolf map files
//             }
//         }
//
//         // Original map seems to have 100 maps placed
//         var numMaps = (headerFile.Length - position) / sizeof(int);
//
//         var headerOffsets = Converters.ByteArrayToInt32Array(headerFile.Skip(position).ToArray(), length: numMaps);
//
//         var headerData = new List<MapHeaderData>(headerOffsets.Length);
//
//         for (var index = 0; index < headerOffsets.Length; index++)
//         {
//             headerData.Add(new MapHeaderData
//             {
//                 Index = index,
//                 MapHeaderSegmentPosition = headerOffsets[index]
//             });
//         }
//
//         return new MapHeaderList
//         {
//             HeaderData = headerFile,
//             NumPlanes = numPlanes,
//             MapHeaders = headerData,
//             RLEWTag = rlewTag
//         };
//     }
//
//     public List<MapData> GetMapDataList()
//     {
//         var mapHeaderList = GetMapHeaderList();
//         var headerData = mapHeaderList.MapHeaders;
//         var data = File.ReadAllBytes(mapFilePath);
//
//         // Some times the map header is padded with extra values,
//         // Drop them, and re-calculate the count of maps
//         var numMaps = headerData.Count(x => x.MapHeaderSegmentPosition > 0);
//
//         var maps = new List<MapData>(numMaps);
//
//         // The last map contains the map metadata at the end of the file, so this should apply to all maps
//         // as the metadata is the same size for each, we can safely determine numPlanes, and this can determine
//         // the size of the map's name as well (usually 16 single-byte chars, or 32 single-byte chars via WDC)
//         var mapMetadataSize = data.Length - headerData.Last(x => x.MapHeaderSegmentPosition > 0).MapHeaderSegmentPosition;
//
//         for (var i = 0; i < numMaps; i++)
//         {
//             var position = headerData[i].MapHeaderSegmentPosition;
//             var nextHeaderPosition = i < numMaps - 1 ? headerData[i + 1].MapHeaderSegmentPosition : data.Length;
//             var chunkLength = nextHeaderPosition - position;
//
//             var rawMetadata = data.Skip(position).Take(mapMetadataSize).ToArray();
//             maps.Add(new MapData
//             {
//                 Index = headerData[i].Index,
//                 MapMetadata = rawMetadata,
//                 HeaderMetadata = MapHeaderMetadata.Build(rawMetadata, mapHeaderList.NumPlanes)
//             });
//         }
//
//         return maps;
//     }
// }
