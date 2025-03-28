using System.Buffers.Binary;

namespace LittleAGames.PFWolf.Common.FileLoaders;

public static class PngFileLoader
{
    private static readonly int ihdr = MakeId([(byte)'I', (byte)'H', (byte)'D', (byte)'R']);
    private static readonly int idat = MakeId([(byte)'I', (byte)'D', (byte)'A', (byte)'T']);
    private static readonly int iend = MakeId([(byte)'I', (byte)'E', (byte)'N', (byte)'D']);
    private static readonly int grab = MakeId([(byte)'g', (byte)'r', (byte)'A', (byte)'b']);
    private static readonly int plte = MakeId([(byte)'P', (byte)'L', (byte)'T', (byte)'E']);
    private static readonly int trns = MakeId([(byte)'t', (byte)'R', (byte)'N', (byte)'S']);
    private static readonly int alph = MakeId([(byte)'a', (byte)'l', (byte)'P', (byte)'h']);
    public static byte[] Create()
    {
        throw new NotImplementedException();
    }

    private static int MakeId(byte[] check)
    {
        return ((check[0]) | ((check[1]) << 8) | ((check[2]) << 16) | ((check[3]) << 24));
        //return ((check[3]) | ((check[2]) << 8) | ((check[1]) << 16) | ((check[0]) << 24));
    }
    
    public static byte[]? Load(byte[] rawData, string name)
    {
        int i;
        for (i = 0; i < rawData.Length-4; i++)
        {
            var byteCompare = MakeId(rawData.Skip(i).Take(sizeof(int)).ToArray());
            
            if (byteCompare == ihdr)
            {
                i += sizeof(int);
                break;
            }
        }

        // IHDR not found
        if (i >= rawData.Length-3)
        {
            return null;
        }

        var width = BinaryPrimitives.ReverseEndianness(BitConverter.ToInt32(rawData.Skip(i).Take(sizeof(int)).ToArray()));
        i += sizeof(int);
        
        var height = BinaryPrimitives.ReverseEndianness(BitConverter.ToInt32(rawData.Skip(i).Take(sizeof(int)).ToArray()));
        i += sizeof(int);
        
        byte bitDepth = rawData.Skip(i).First();
        byte colorType = rawData.Skip(++i).First();
        byte compression = rawData.Skip(++i).First();
        byte filter = rawData.Skip(++i).First();
        byte interlace = rawData.Skip(++i).First();
        
        if (compression != 0 || filter != 0 || interlace > 1)
        {
            return null;
        }
        if (((1 << colorType) & 0x5D) == 0)
        {
            return null;
        }
        if (((1 << bitDepth) & 0x116) == 0)
        {
            return null;
        }

        i = 33;
        var length = BinaryPrimitives.ReverseEndianness(BitConverter.ToInt32(rawData.Skip(i).Take(sizeof(int)).ToArray()));
        i += sizeof(int);
        
        var id = BitConverter.ToInt32(rawData.Skip(i).Take(sizeof(int)).ToArray());
        i += sizeof(int);

        while (id != idat && id != iend)
        {
            if (id == grab)
            {
                var hotX = BitConverter.ToInt32(rawData.Skip(i).Take(4).ToArray());
                i += sizeof(int);
                var hotY = BitConverter.ToInt32(rawData.Skip(i).Take(4).ToArray());
                i += sizeof(int);

                if (hotX < -32768 || hotX > 32767)
                {
                    Console.WriteLine($"X-Offset for PNG texture {name} is bad: {hotX}");
                    hotX = 0;
                }
                
                if (hotY < -32768 || hotY > 32767)
                {
                    Console.WriteLine($"Y-Offset for PNG texture {name} is bad: {hotY}");
                    hotY = 0;
                }
                
                //LeftOffset = hotX;
                //TopOffset = hotY;
            }
            else if (id == plte)
            {
                var paletteSize = Math.Min(length / 3, 256);
                var palette = rawData.Skip(i).Take(paletteSize * 3).ToArray();
                i += paletteSize * 3;
                
                for (var p = paletteSize - 1; p >= 0; --p)
                {
                    //Palette->palette[p] = MAKERGB(Palette->pngpal[p][0], Palette->pngpal[p][1], Palette->pngpal[p][2]);
                }
            }
            else if (id == trns)
            {
                ;
            }
            else if (id == alph)
            {
                ;
            }
            else
            {
                i += length;
            }

            i += sizeof(int); // Skip CRC
            length = BinaryPrimitives.ReverseEndianness(BitConverter.ToInt32(rawData.Skip(i).Take(sizeof(int)).ToArray()));
            i += sizeof(int);
            
            id = iend;
            if (i >= rawData.Length)
                break;
            id = BitConverter.ToInt32(rawData.Skip(i).Take(sizeof(int)).ToArray());
            i += sizeof(int);
        }

        var startOfIDat = i - 8;
        bool bMasked = false;
        switch (colorType)
        {
            case 4:		// Grayscale + Alpha
            case 6:		// RGB + Alpha
                bMasked = true;
                break;

            case 2:		// RGB
               // bMasked = HaveTrans;
                break;
        }

        MakePaletteMap ();

        MakeTexture(); // Called at another time to not load in right away
        // TODO: Find the IHDR tag of the file
        return [];
    }

    private static void MakePaletteMap()
    {
        
    }

    public static void MakeTexture()
    {
        
    }
    
    
    public static byte[] DummyPng()
    {
        return
        [
            137,(byte)'P',(byte)'N',(byte)'G',13,10,26,10,
            0,0,0,13,(byte)'I',(byte)'H',(byte)'D',(byte)'R',
            0,0,0,1,0,0,0,1,8,0,0,0,0,0x3a,0x7e,0x9b,0x55,
            0,0,0,10,(byte)'I',(byte)'D',(byte)'A',(byte)'T',
            104,222,99,96,0,0,0,2,0,1,0x9f,0x65,0x0e,0x18
        ];
    }
}