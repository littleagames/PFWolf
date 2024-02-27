using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Compression
{
    public interface ICompression
    {
        byte[] Expand(byte[] source, int length);
    }

    internal class HuffmanCompression : ICompression
    {
        private byte[] DictionaryFile { get; }

        public HuffmanCompression(byte[] dictionaryFile)
        {
            DictionaryFile = dictionaryFile;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct huffnode
        {
            public ushort bit0;
            public ushort bit1;
        }

        private huffnode[] GetTable()
        {
            var grHuffman = new huffnode[255];
            var grHuffmanSize = (Marshal.SizeOf(typeof(huffnode)) * grHuffman.Length);
            int i, j;
            for (i = 0, j = 0; i < grHuffmanSize; j++, i += Marshal.SizeOf(typeof(huffnode)))
            {
                var huffBytes = DictionaryFile.Skip(i).Take(Marshal.SizeOf(typeof(huffnode))).ToArray();
                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(huffnode)));
                try
                {
                    Marshal.Copy(huffBytes, 0, ptr, Marshal.SizeOf(typeof(huffnode)));
                    grHuffman[j] = (huffnode)Marshal.PtrToStructure(ptr, typeof(huffnode));
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }

            return grHuffman;
        }

        public byte[] Expand(byte[] source, int length)
        {
            var hufftable = GetTable();

            byte[] dest = new byte[length];

            if (length == 0)
            {
                throw new Exception("CAL_HuffExpand: length or pictableLength is 0");
            }

            // Set the head node to the last index of the hufftable
            var headptr = hufftable[254];        // head node is always node 254

            // set the end value
            var destIndex = 0;

            // take the next value from source
            int sourceIndex = 0;
            byte val = source[sourceIndex++];
            byte mask = 1;
            ushort nodeval;

            // Set the huffptr to the head of the tree
            var huffptr = headptr;
            while (true)
            {
                // check if the val matches the mask bit (2^n)
                if ((val & mask) == 0)
                    nodeval = huffptr.bit0;
                else
                    nodeval = huffptr.bit1;
                if (mask == 0x80) // if mask is 128, 2^7
                {
                    val = source[sourceIndex++]; // get next source void
                    mask = 1;           // start mask back to 1
                }
                else mask <<= 1; // bit shift mask left by 1

                if (nodeval < 256)
                {
                    dest[destIndex++] = (byte)nodeval;
                    huffptr = headptr; // start huffptr back at head
                    if (destIndex >= length) break; // if end is reached, done
                }
                else
                {
                    // set ptr to table[nodeval- 256]
                    huffptr = hufftable[nodeval - 256];
                }
            }

            return dest;
        }
    }
}
