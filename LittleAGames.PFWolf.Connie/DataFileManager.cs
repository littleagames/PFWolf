
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

//    public void LoadDataFiles()
//    {
//        for (var i = 1; i < numLumps; i++)
//        {
//            var data = graphicsFile.Skip(graphicStartPositions[i]).Take(size).ToArray();

//            var tile8Position = numFonts + numPics + 1;
//            else
//            {
//                var expanded = BitConverter.ToInt32(data.Take(4).ToArray());
//                var decompressedData = huffman.Expand2(data.Skip(4).ToArray());

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
//            }
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
