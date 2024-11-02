namespace LittleAGames.PFWolf.SDK.Compression;

public class RLEWCompression : ICompression<ushort>
{
    private const ushort RLEW_MARKER = 0xABCD;  // Chosen marker for indicating RLE sequences

    public ushort[] Expand(byte[] source)
    {
        List<ushort> data = new List<ushort>();

        using (MemoryStream ms = new MemoryStream(source))
        using (BinaryReader reader = new BinaryReader(ms))
        {
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                ushort value = reader.ReadUInt16();

                // Check for the RLEW marker
                if (value == RLEW_MARKER)
                {
                    // If marker is found, the next word is the run length, and the next is the repeated value
                    ushort runLength = reader.ReadUInt16();
                    ushort repeatedValue = reader.ReadUInt16();

                    // Add the repeated value to the list `runLength` times
                    for (int j = 0; j < runLength; j++)
                    {
                        data.Add(repeatedValue);
                    }
                }
                else
                {
                    // If no marker, add the value directly
                    data.Add(value);
                }
            }
        }

        return data.ToArray();
    }

    public byte[] Compress(ushort[] source)
    {
        using (MemoryStream ms = new MemoryStream())
        using (BinaryWriter writer = new BinaryWriter(ms))
        {
            int i = 0;
            while (i < source.Length)
            {
                ushort value = source[i];
                int runLength = 1;

                // Count how many times the current value repeats consecutively
                while (i + runLength < source.Length && source[i + runLength] == value)
                {
                    runLength++;
                }

                // If run length is greater than 1, use the RLEW_MARKER to indicate compression
                if (runLength > 1)
                {
                    writer.Write(RLEW_MARKER);   // Write the RLEW marker
                    writer.Write((ushort)runLength);  // Write the count
                    writer.Write(value);         // Write the repeated value
                    i += runLength;              // Skip past the repeated values
                }
                else
                {
                    // If there's no repetition, write the value directly
                    writer.Write(value);
                    i++;
                }
            }

            return ms.ToArray();
        }
    }
}