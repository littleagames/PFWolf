using System.Runtime.InteropServices;

namespace Engine.Tools;

internal class ByteToStructHelpers
{
    internal static T ByteArrayToStucture<T>(byte[] bytes) where T : struct
    {
        var structBytes = bytes.Take(Marshal.SizeOf(typeof(T))).ToArray();
        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)));
        try
        {
            Marshal.Copy(structBytes, 0, ptr, Marshal.SizeOf(typeof(T)));
            return (T)Marshal.PtrToStructure(ptr, typeof(T));
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    internal static T[] ByteArrayToStuctureArray<T>(byte[] bytes, int lengthOfT) where T : struct
    {
        var outT = new T[lengthOfT];
        var size = Marshal.SizeOf(typeof(T)) * lengthOfT;
        int i, j;
        for (i = 0, j = 0; i < size; j++, i += Marshal.SizeOf(typeof(T)))
        {
            var itemBytes = bytes.Skip(i).Take(Marshal.SizeOf(typeof(T))).ToArray();
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)));
            try
            {
                Marshal.Copy(itemBytes, 0, ptr, Marshal.SizeOf(typeof(T)));
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
