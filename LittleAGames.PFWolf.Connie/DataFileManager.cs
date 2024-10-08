
////public class Sprite
////{
////    public int Index { get; set; }
////    public string Name { get => $"Sprite {Index:000}"; }
////    public byte[] Data { get; set; } = [];
////    public int OffsetX { get; private set; }
////    public int OffsetY { get; private set; }
////    public int Width { get; private set; }
////    public int Height { get; private set; }

////    public void Calc()
////    {
////        var leftpix = BitConverter.ToUInt16(Data, 0);
////        var rightpix = BitConverter.ToUInt16(Data, 2);
////        Width = rightpix - leftpix;

////        var dataOffsets = Converters.ByteArrayToUInt16Array(Data.Skip(4).ToArray(), Width);
////        // TODO: get offsets, and width/height
////        // TODO: depending on what uses this, it'll draw how it sees fit?
////    }
////}


////    //private void DrawWall(byte[] block)
////    //{
////    //    VideoLayerManager.Instance.DrawBlock(0, 0, block);
////    //    VideoLayerManager.Instance.UpdateScreen();
////    //    //var color = 255;
////    //    //for (var x = 0; x < 64; x++)
////    //    //{
////    //    //    for (var y = 0; y < 64; y++)
////    //    //    {
////    //    //        color = block[y * 64 + x];

////    //    //        if (color == 255)
////    //    //        {
////    //    //            Console.Write("  ");
////    //    //        }
////    //    //        else
////    //    //        {
////    //    //            Console.Write($"{color:X2}");
////    //    //        }
////    //    //    }
////    //    //    Console.WriteLine();
////    //    //}
////    //}

////    //private void DrawSprite(Sprite sprite)
////    //{
////    //    byte color = 255;
////    //    var block = new byte[64*64]; // 64x64
////    //    for(var i = 0; i < block.Length;i++)
////    //    {
////    //        block[i] = 255;
////    //    }

////    //    for (var x = sprite.LeftPix; x < sprite.RightPix; x++)
////    //    {
////    //        var dataOfs = sprite.DataOffsets[x-sprite.LeftPix];
////    //        for (var end = BitConverter.ToInt16(sprite.Data, dataOfs) >> 1; end != 0; end = BitConverter.ToInt16(sprite.Data, dataOfs) >> 1)
////    //        {
////    //            var top = BitConverter.ToInt16(sprite.Data, dataOfs + 2);
////    //            var start = BitConverter.ToInt16(sprite.Data, dataOfs + 4) >> 1;

////    //            for (; start < end; start++)
////    //            {
////    //                // draw vertical segment
////    //                color = sprite.Data[start + top];
////    //                // get x,y coords
////    //                var y = start;
////    //                block[y * 64 + x] = color;
////    //            }

////    //            dataOfs += 6;
////    //        }
////    //    }

////    //    VideoLayerManager.Instance.DrawBlock(0, 0, block);
////    //    VideoLayerManager.Instance.UpdateScreen();
////    //    return;
////    //}
////}
//using LittleAGames.PFWolf.Common.Compression;
//using Microsoft.VisualBasic;
//using System.Runtime.InteropServices;
//using System.Text.Json;
//using Wolf3D_CSharp.Compression;
//using Wolf3D_CSharp.Data.BaseFileLoader.Tools;
//using Wolf3D_CSharp.Tools;

//namespace Wolf3D_CSharp;

//public class VgaGraphFileLoader
//{
//    [StructLayout(LayoutKind.Sequential, Pack = 1)]
//    private struct pictabletype
//    {
//        public short width, height;
//    }

//    public void LoadDataFiles()
//    {
//        // Try loading the VGA files

//        const string graphicsFileName = "vgagraph";
//        const string graphicsHeaderFileName = "vgahead";
//        const string graphicsDictionaryFileName = "vgadict";

//        // Load dictionary
//        var dictionaryFile = File.ReadAllBytes($"{Constants.GameFilesDirectory}{graphicsDictionaryFileName}.{Constants.FileExtension}");
//        Console.WriteLine($"Dictionary file size: {dictionaryFile.Length}");

//        // Load header
//        var headerLength = new FileInfo($"{Constants.GameFilesDirectory}{graphicsHeaderFileName}.{Constants.FileExtension}").Length;
//        Console.WriteLine($"Header length: {headerLength}");
//        var headerFile = File.ReadAllBytes($"{Constants.GameFilesDirectory}{graphicsHeaderFileName}.{Constants.FileExtension}");
//        Console.WriteLine($"Header file size: {headerFile.Length}");

//        // Expand and Load graphics
//        var graphicsFile = File.ReadAllBytes($"{Constants.GameFilesDirectory}{graphicsFileName}.{Constants.FileExtension}");
//        Console.WriteLine($"Graphics file size: {graphicsFile.Length}");

//        const int headerChunk = 3; // 3 is for each byte (we read 3 bytes of data)

//        var numLumps = (headerFile.Length / headerChunk);

//        var graphicStartPositions = new int[numLumps];

//        // Header has 24-bit integers
//        try
//        {
//            for (var i = 0; i < graphicStartPositions.Length; i++)
//            {
//                var d0 = headerFile[i * headerChunk];
//                var d1 = headerFile[i * headerChunk + 1];
//                var d2 = headerFile[i * headerChunk + 2];
//                var val = d0 | d1 << 8 | d2 << 16; // Little endian
//                graphicStartPositions[i] = val == 0x00FFFFFF ? -1 : val;
//                Console.WriteLine($"Graphic #{i} start: {graphicStartPositions[i]}");
//            }

//            Console.WriteLine($"Num Chunks: {numLumps}");
//        }
//        catch (Exception e)
//        {
//            Console.WriteLine(e);
//            return;
//        }

//        var huffman = new HuffmanCompression(dictionaryFile);
//        var lumps = new List<VgaLump>(numLumps);
//        var numFonts = 0;

//        // Struct pic data
//        // 4 bytes = size of picdata
//        // remainder is pictabletype compressed data
//        var pictableSize = BitConverter.ToInt32(graphicsFile, graphicStartPositions[0]);
//        var numPics = pictableSize / Marshal.SizeOf(typeof(pictabletype));

//        // Is the chunkcomplen 386 or 390? and does stripping 4 bytes off of the end cause issues for anything else
//        // In this case, 390 gives us 586 in compout2 and that's an additional 8 bytes that we don't care to have.
//        // Original code has | 4 bytes = len | 390 bytes as "compseg" | as part of STRUCTPIC
//        // But the last 4 bytes are all 0s
//        var compSegment = graphicsFile.Take(graphicStartPositions[1] - 4).Skip(sizeof(int)).ToArray(); // Skip first 4 bytes (pic table size)

//        var compout = huffman.Expand(compSegment, pictableSize);
//        var compout2 = huffman.Expand2(compSegment);

//        var pictable = ByteToStructHelpers.ByteArrayToStructureArray<pictabletype>(compout, numPics);
//        var fontCheckComplete = false;

//        for (var i = 1; i < numLumps; i++)
//        {
//            // Maybe assumed that the layout is
//            // STRUCT PIC data
//            // FONTS
//            // PICTURES
//            // TILE8s
//            // etc

//            var end = graphicsFile.Length;
//            if (i < numLumps - 1)
//            {
//                end = graphicStartPositions[i + 1];
//            }

//            var size = end - graphicStartPositions[i];

//            if (size <= 0) continue;

//            var data = graphicsFile.Skip(graphicStartPositions[i]).Take(size).ToArray();

//            var tile8Position = numFonts + numPics + 1;
//            // Determine if last lump was a font
//            if (!fontCheckComplete && BuildFontLump(data, i, huffman, out var lump))
//            {
//                numFonts++;
//                lumps.Add(lump);
//                Console.WriteLine($"Added font {numFonts}");
//            }
//            else if (i <= (numPics + numFonts) && BuildPictureLump(data, pictable[i - numFonts - 1], i, huffman, out lump))
//            {
//                if (!fontCheckComplete)
//                    fontCheckComplete = true;

//                lumps.Add(lump);
//                Console.WriteLine($"Added picture {i - numFonts - 1}");
//            }
//            else if (i == tile8Position && BuildTile8Lump(data, i, huffman, out lump))
//            {
//                lumps.Add(lump);
//                Console.WriteLine($"Added TILE8");
//            }
//            else
//            {
//                var expanded = BitConverter.ToInt32(data.Take(4).ToArray());
//                var decompressedData = huffman.Expand2(data.Skip(4).ToArray());

//                // Check if palette?
//                // 256*3 bytes
//                if (decompressedData.Length == 256 * 3)
//                {
//                    var screenLump = new VgaLump
//                    {
//                        LumpName = $"VGAPALETTE{i:D5}",
//                        //Data = decompressedData
//                    };
//                    lumps.Add(screenLump);
//                    continue;
//                }

//                // Check if ENDSCREEN (4000 bytes?)
//                // 80 x 25
//                // ASCII, foreground, and background color (2 + 1 + 1 byte)
//                if (decompressedData.Length == 4008)
//                {

//                    var screenLump = new VgaLump
//                    {
//                        LumpName = $"VGASCREEN{i:D5}",
//                        //Data = decompressedData
//                    };
//                    lumps.Add(screenLump);
//                    continue;
//                }

//                // Check if DEMO?
//                var gameMapNumber = decompressedData[0];
//                var length = BitConverter.ToInt16(decompressedData.Skip(1).Take(2).ToArray());
//                if (length == decompressedData.Length || length == data.Length)
//                {
//                    var demoData = decompressedData.Skip(4);
//                    // First byte (map)
//                    // Second 2 bytes (length)
//                    var screenLump = new VgaLump
//                    {
//                        LumpName = $"VGADEMO{i:D5}",
//                        //Data = decompressedData
//                    };
//                    lumps.Add(screenLump);
//                    continue;
//                }

//                // Check if TEXT
//                var text = System.Text.Encoding.ASCII.GetString(decompressedData).ToCharArray();

//                // TODO: Spear has a text file, but no starter ^P, so I need to just check the whole file for valid ascii
//                // So that'd be 32 - ??? and that \0 is at the end only. (if there are several, it's not text)
//                if (text.Take(2).ToString() == "^P")
//                {
//                    var textLump = new VgaLump
//                    {
//                        LumpName = $"VGATEXT{i:D5}",
//                        //Data = decompressedData
//                    };
//                    lumps.Add(textLump);
//                    continue;
//                }


//                var otherLump = new VgaLump
//                {
//                    LumpName = $"VGAOTHER{i:D5}",
//                    //Data = decompressedData
//                };
//                lumps.Add(otherLump);
//            }
//        }

//        Console.WriteLine();
//        Console.WriteLine("=====================================");
//        Console.WriteLine("=  LUMPS");
//        Console.WriteLine("=====================================");

//        foreach (var lump in lumps)
//        {
//            Console.WriteLine($"Lump: {lump.LumpName}");
//        }
//    }

//    private bool BuildFontLump(byte[] data, int lumpNumber, HuffmanCompression huffman, out VgaLump lump)
//    {
//        var expanded = BitConverter.ToInt32(data.Take(4).ToArray());
//        var decompressedData = huffman.Expand(data.Skip(4).ToArray(), expanded);
//        var decompressedData2 = huffman.Expand2(data.Skip(4).ToArray());
//        // minimum font header size
//        if (decompressedData.Length <= 770)
//        {
//            lump = null!;
//            return false;
//        }
//        var height = BitConverter.ToInt16(decompressedData);

//        // Max font height is 255
//        if (height > 255)
//        {
//            lump = null!;
//            return false;
//        }

//        short[] location = new short[256];
//        Buffer.BlockCopy(decompressedData, sizeof(short), location, 0, sizeof(short) * 256);

//        // If any of the locations are not within the size of the blob
//        if (location.ToList().Any(l => l > decompressedData.Length))
//        {
//            lump = null!;
//            return false;
//        }

//        byte[] widths = new byte[256];
//        Buffer.BlockCopy(decompressedData, sizeof(short) + sizeof(short) * 256, widths, 0, sizeof(byte) * 256);

//        // If any of the locations are not within the size of the blob
//        if (widths.ToList().Any(w => w > decompressedData.Length))
//        {
//            lump = null!;
//            return false;
//        }

//        var fontLump = new VgaFontLump
//        {
//            LumpName = $"VGAFONT{lumpNumber:D5}",
//            Height = height,
//            Widths = widths
//        };

//        for (var ascii = 0; ascii < 256; ascii++)
//        {
//            byte[] fontData = decompressedData.Skip(location[ascii]).Take(sizeof(byte) * widths[ascii] * height)
//                .ToArray();
//            fontLump.Data[ascii] = fontData;
//        }

//        lump = fontLump;
//        return true;
//    }

//    private bool BuildPictureLump(byte[] data, pictabletype picData, int lumpNumber, HuffmanCompression huffman, out VgaLump lump)
//    {
//        var expanded = BitConverter.ToInt32(data.Take(4).ToArray());
//        //var decompressedData = huffman.Expand(data.Skip(4).ToArray(), expanded);
//        var decompressedData = huffman.Expand2(data.Skip(4).ToArray());

//        lump = new VgaPictureLump
//        {
//            LumpName = $"VGAPICT{lumpNumber:D5}",
//            Width = picData.width,
//            Height = picData.height,
//            Data = decompressedData
//        };

//        return true;
//    }

//    private bool BuildTile8Lump(byte[] data, int lumpNumber, HuffmanCompression huffman, out VgaLump lump)
//    {
//        // The tile 8 doesn't contain a size at the start
//        // TODO: This isn't quite right
//        //var expanded = BitConverter.ToInt32(data.Take(4).ToArray());
//        var size = 64 * 35; // current tile 8 size, but it could always change???
//        var decompressedData = huffman.Expand(data.ToArray(), size);
//        var decompressedData2 = huffman.Expand2(data.ToArray());

//        lump = new VgaTile8Lump
//        {
//            LumpName = $"VGATILE{lumpNumber:D5}",
//            Width = 0,
//            Height = 0,
//            Data = decompressedData
//        };
//        return true;
//    }

//    private class VgaLump
//    {
//        public string LumpName { get; set; } = null!;
//        public int Position { get; set; }
//        public int CompressedSize { get; set; }
//        public int DecompressedSize { get; set; }
//    }

//    private class VgaTile8Lump : VgaLump
//    {
//        public int Height { get; set; }
//        public int Width { get; set; }
//        public byte[] Data { get; set; } = null!;
//    }

//    private class VgaFontLump : VgaLump
//    {
//        public int Height { get; set; }
//        public byte[] Widths { get; set; } = new byte[256];
//        public byte[][] Data { get; set; } = new byte[256][];
//    }

//    private class VgaPictureLump : VgaLump
//    {
//        public int Height { get; set; }
//        public int Width { get; set; }
//        public byte[] Data { get; set; } = null!;
//    }
//}
