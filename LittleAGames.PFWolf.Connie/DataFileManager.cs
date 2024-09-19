
//public class Sprite
//{
//    public int Index { get; set; }
//    public string Name { get => $"Sprite {Index:000}"; }
//    public byte[] Data { get; set; } = [];
//    public int OffsetX { get; private set; }
//    public int OffsetY { get; private set; }
//    public int Width { get; private set; }
//    public int Height { get; private set; }

//    public void Calc()
//    {
//        var leftpix = BitConverter.ToUInt16(Data, 0);
//        var rightpix = BitConverter.ToUInt16(Data, 2);
//        Width = rightpix - leftpix;

//        var dataOffsets = Converters.ByteArrayToUInt16Array(Data.Skip(4).ToArray(), Width);
//        // TODO: get offsets, and width/height
//        // TODO: depending on what uses this, it'll draw how it sees fit?
//    }
//}


//    //private void DrawWall(byte[] block)
//    //{
//    //    VideoLayerManager.Instance.DrawBlock(0, 0, block);
//    //    VideoLayerManager.Instance.UpdateScreen();
//    //    //var color = 255;
//    //    //for (var x = 0; x < 64; x++)
//    //    //{
//    //    //    for (var y = 0; y < 64; y++)
//    //    //    {
//    //    //        color = block[y * 64 + x];

//    //    //        if (color == 255)
//    //    //        {
//    //    //            Console.Write("  ");
//    //    //        }
//    //    //        else
//    //    //        {
//    //    //            Console.Write($"{color:X2}");
//    //    //        }
//    //    //    }
//    //    //    Console.WriteLine();
//    //    //}
//    //}

//    //private void DrawSprite(Sprite sprite)
//    //{
//    //    byte color = 255;
//    //    var block = new byte[64*64]; // 64x64
//    //    for(var i = 0; i < block.Length;i++)
//    //    {
//    //        block[i] = 255;
//    //    }

//    //    for (var x = sprite.LeftPix; x < sprite.RightPix; x++)
//    //    {
//    //        var dataOfs = sprite.DataOffsets[x-sprite.LeftPix];
//    //        for (var end = BitConverter.ToInt16(sprite.Data, dataOfs) >> 1; end != 0; end = BitConverter.ToInt16(sprite.Data, dataOfs) >> 1)
//    //        {
//    //            var top = BitConverter.ToInt16(sprite.Data, dataOfs + 2);
//    //            var start = BitConverter.ToInt16(sprite.Data, dataOfs + 4) >> 1;

//    //            for (; start < end; start++)
//    //            {
//    //                // draw vertical segment
//    //                color = sprite.Data[start + top];
//    //                // get x,y coords
//    //                var y = start;
//    //                block[y * 64 + x] = color;
//    //            }

//    //            dataOfs += 6;
//    //        }
//    //    }

//    //    VideoLayerManager.Instance.DrawBlock(0, 0, block);
//    //    VideoLayerManager.Instance.UpdateScreen();
//    //    return;
//    //}
//}