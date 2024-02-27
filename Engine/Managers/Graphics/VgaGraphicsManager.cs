using Engine.Compression;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Engine.Managers.Graphics;

/// <summary>
/// Handles the work of reading the base files and loading those into a basic format to be consumed
/// </summary>
public class VgaGraphicsManager
{
    private static volatile VgaGraphicsManager? _instance = null;
    private static object syncRoot = new object();

    public byte[][] grsegs = new byte[Constants.NumChunks][];
    public pictabletype[] pictable = new pictabletype[Constants.NumPics];

    private VgaGraphicsManager()
    {
    }

    public static VgaGraphicsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // only create a new instance if one doesn't already exist.
                lock (syncRoot)
                {
                    // use this lock to ensure that only one thread can access
                    // this block of code at once.
                    if (_instance == null)
                    {
                        _instance = new VgaGraphicsManager();
                    }
                }
            }

            return _instance;
        }
    }


    public struct pictabletype
    {
        public short width, height;
    }

    public void LoadDataFiles()
    {
        // Try loading the VGA files
        const string fileExtension = "wl6";

        const string tempDirectory = "D:\\PFWolf-CSharp\\";

        const string graphicsFileName = "vgagraph";
        const string graphicsHeaderFileName = "vgahead";
        const string graphicsDictionaryFileName = "vgadict";

        // Load dictionary
        byte[] dictionaryFile = File.ReadAllBytes($"{tempDirectory}{graphicsDictionaryFileName}.{fileExtension}");


        // Load header
        var headerLength = new FileInfo($"{tempDirectory}{graphicsHeaderFileName}.{fileExtension}").Length;

        const int magic3 = 3;

        var grstarts = new int[Constants.NumChunks + 1];
        var expectedSize = grstarts.Length; // NUMCHUNKS

        if (headerLength / magic3 != expectedSize) // What does the 3 mean? Why 3?
        {
            throw new Exception("VGA File is not right");
        }

        byte[] headerFile = File.ReadAllBytes($"{tempDirectory}{graphicsHeaderFileName}.{fileExtension}");

        for (var i = 0; i != grstarts.Length; i++)
        {
            var d0 = headerFile[i * magic3];
            var d1 = headerFile[i * magic3 + 1];
            var d2 = headerFile[i * magic3 + 2];
            int val = d0 | d1 << 8 | d2 << 16;
            grstarts[i] = val == 0x00FFFFFF ? -1 : val;
        }

        // Expand and Load graphics
        byte[] graphicsFile = File.ReadAllBytes($"{tempDirectory}{graphicsFileName}.{fileExtension}");

        var filePos = grstarts[Constants.StructPic];
        int chunkcomplen;//, chunkexplen;

        //chunkexplen = BitConverter.ToInt32(graphicsFile.Skip(filePos).Take(4).ToArray(), 0);
        chunkcomplen = grstarts[Constants.StructPic + 1] - grstarts[Constants.StructPic] - 4;
        var compseg = graphicsFile.Skip(filePos + 4).Take(chunkcomplen).ToArray();


        var pictableSize = Marshal.SizeOf(typeof(pictabletype)) * pictable.Length;

        var compression = new HuffmanCompression(dictionaryFile);
        var destTable = compression.Expand(compseg, pictableSize);

        pictable = ByteArrayToStuctureArray<pictabletype>(destTable, pictable.Length);
        //free(compseg);

        CA_CacheGrChunks(grstarts, grsegs, graphicsFile, compression);

    }

    void CA_CacheGrChunks(int[] grstarts, byte[][] grsegs, byte[] graphicsFile, ICompression compression)
    {
        int pos, compressed;
        //int* bufferseg;
        //int* source;
        int chunk, next;

        for (chunk = Constants.StructPic + 1; chunk < Constants.NumChunks; chunk++)
        {
            // If there is data in the grsegs chunk, skip it
            if (grsegs[chunk] != null && grsegs[chunk].Length > 0)
                continue;                             // already in memory

            //
            // load the chunk into a buffer
            //
            pos = grstarts[chunk];

            if (pos < 0)                              // $FFFFFFFF start is a sparse tile
                continue;

            next = chunk + 1;

            while (grstarts[next] == -1)           // skip past any sparse tiles
                next++;

            compressed = grstarts[next] - pos; // TODO: I'm off by 4 each time

            // load into source, an int32 worth of byte data (so 4?)
            //var source = BitConverter.ToInt32(graphicsFile.Skip(pos).Take(4).ToArray(), 0);
            var source = graphicsFile.Skip(pos).Take(compressed).ToArray();

            var bufferseg = new int[compressed];// SafeMalloc(compressed);
            //source = bufferseg;

            //read(grhandle, source, compressed);

            CAL_ExpandGrChunk(chunk, source, compression);

            if (chunk >= Constants.StartPics && chunk < Constants.StartExterns)
                CAL_DeplaneGrChunk(chunk);

            //free(bufferseg);
        }
    }
    void CAL_ExpandGrChunk(int chunk, byte[] source, ICompression compression)
    {
        int expanded;
        int sourceIndex = 0;

        if (chunk >= Constants.StartTile8s && chunk < Constants.StartExterns)
        {
            //
            // expanded sizes of tile8/16/32 are implicit
            //

            const int BLOCK = 64;
            const int MASKBLOCK = 128;

            //if (chunk < STARTTILE8M)          // tile 8s are all in one chunk!
            //    expanded = BLOCK * NUMTILE8;
            //else if (chunk < STARTTILE16)
            //    expanded = MASKBLOCK * NUMTILE8M;
            //else if (chunk < STARTTILE16M)    // all other tiles are one/chunk
            //    expanded = BLOCK * 4;
            //else if (chunk < STARTTILE32)
            //    expanded = MASKBLOCK * 4;
            //else if (chunk < STARTTILE32M)
            //    expanded = BLOCK * 16;
            //else
            expanded = MASKBLOCK * 16;
        }
        else
        {
            //
            // everything else has an explicit size longword
            //
            expanded = BitConverter.ToInt32(source.Skip(sourceIndex++).Take(4).ToArray()); // TODO: This needs 4 bytes into expanded
        }

        //
        // allocate final space and decompress it
        //
        //grsegs[chunk] = new byte[expanded];// SafeMalloc(expanded);

        grsegs[chunk] = compression.Expand(source.Skip(4).ToArray(), expanded);
    }
    void CAL_DeplaneGrChunk(int chunk)
    {
        int i;
        short width, height;

        if (chunk == Constants.StartTile8s)
        {
            width = height = 8;

            for (i = 0; i < Constants.NumTile8; i++)
                VL_DePlaneVGA(grsegs[chunk]/*[(i * (width * height))]*/, width, height);
        }
        else
        {
            width = pictable[chunk - Constants.StartPics].width;
            height = pictable[chunk - Constants.StartPics].height;

            VL_DePlaneVGA(grsegs[chunk], width, height);
        }
    }
    void VL_DePlaneVGA(byte[] source, int width, int height)
    {
        int x, y, plane;
        ushort size, pwidth;
        byte[] temp, dest, srcline;

        size = (ushort)(width * height);

        if ((width & 3) != 0)
            throw new Exception("DePlaneVGA: width not divisible by 4!");

        temp = new byte[size];// SafeMalloc(size);

        //
        // munge pic into the temp buffer
        //
        srcline = source;
        var srcLineIndex = 0;
        pwidth = (ushort)(width >> 2);

        for (plane = 0; plane < 4; plane++)
        {
            dest = temp;

            for (y = 0; y < height; y++)
            {
                for (x = 0; x < pwidth; x++)
                    dest[width * y + (x << 2) + plane] = srcline[srcLineIndex++];
            }
        }

        //
        // copy the temp buffer back into the original source
        //
        Array.Copy(temp, source, size);
    }

    private static T[] ByteArrayToStuctureArray<T>(byte[] bytes, int lengthOfT) where T : struct
    {
        var outT = new T[lengthOfT];
        var grHuffmanSize = Marshal.SizeOf(typeof(T)) * lengthOfT;
        int i, j;
        for (i = 0, j = 0; i < grHuffmanSize; j++, i += Marshal.SizeOf(typeof(T)))
        {
            var huffBytes = bytes.Skip(i).Take(Marshal.SizeOf(typeof(T))).ToArray();
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)));
            try
            {
                Marshal.Copy(huffBytes, 0, ptr, Marshal.SizeOf(typeof(T)));
                outT[j] = (T)Marshal.PtrToStructure(ptr, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        return outT;
    }
}
