namespace LittleAGames.PFWolf.SDK.Compression;

public interface ICompression<T>
{
    T[] Expand(byte[] source);

    byte[] Compress(T[] source);

    // string DisplayTree();
    // string DisplayLeaves();
}