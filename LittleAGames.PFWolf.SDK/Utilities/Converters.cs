namespace LittleAGames.PFWolf.SDK.Utilities;

public static class Converters
{
    public static char[] ByteArrayToCharArray(byte[] bytes, int length)
    {
        char[] result = new char[length];

        for (int i = 0, j = 0; i < bytes.Length && j < length; i += sizeof(char), j++)
        {
            result[j] = BitConverter.ToChar(bytes, i);
        }

        return result;
    }

    public static byte[] UInt16ArrayToByteArray(ushort[] source)
    {
        byte[] target = new byte[source.Length * 2]; 
        Buffer.BlockCopy(source, 0, target, 0, source.Length * 2);
        return target;
    }
    
    public static ushort[] ByteArrayToUInt16Array(byte[] source)
    {
        ushort[] target = new ushort[source.Length / 2 + source.Length % 2];
        Buffer.BlockCopy(source, 0, target, 0, source.Length);
        return target;
    }
    
    public static UInt16[] ByteArrayToUInt16Array(byte[] bytes, int length)
    {
        UInt16[] result = new UInt16[length];

        for (int i = 0, j = 0; i < bytes.Length && j < length; i += sizeof(UInt16), j++)
        {
            result[j] = BitConverter.ToUInt16(bytes, i);
        }

        return result;
    }

    public static UInt32[] ByteArrayToUInt32Array(byte[] bytes, int length)
    {
        UInt32[] result = new UInt32[length];

        for (int i = 0, j = 0; i < bytes.Length; i += sizeof(UInt32), j++)
        {
            result[j] = BitConverter.ToUInt32(bytes, i);
        }

        return result;
    }
    public static Int32[] ByteArrayToInt32Array(byte[] bytes, int length)
    {
        Int32[] result = new Int32[length];

        for (int i = 0, j = 0; i < bytes.Length; i += sizeof(Int32), j++)
        {
            result[j] = BitConverter.ToInt32(bytes, i);
        }

        return result;
    }
}
