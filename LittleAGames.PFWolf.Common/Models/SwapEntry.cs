using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleAGames.PFWolf.Common.Models;

public abstract class SwapEntry
{
    public int Index { get; private set; }
    public byte[] Data { get; private set; } = [];
    public int Size { get => Data?.Length ?? 0; }

    public SwapEntry(byte[] data, int index)
    {
        Index = index;
        Data = data;
    }

    public abstract string Display();
}

public class SparseEntry : SwapEntry
{
    public SparseEntry(int index) : base([], index)
    {
    }

    public override string Display()
        => $"Sparse Entry #{Index}";
}

public class WallEntry : SwapEntry
{
    public WallEntry(byte[] data, int index)
        : base(data, index)
    {
    }

    public override string Display()
        => $"Wall #{Index} Size ({Size} bytes)";
}

public class SpriteEntry : SwapEntry
{
    public SpriteEntry(byte[] data, int index)
        : base(data, index)
    {
    }

    public override string Display()
        => $"Sprite #{Index} Size ({Size} bytes)";
}

public class DigitizedSoundEntry : SwapEntry
{
    public DigitizedSoundEntry(byte[] data, int index)
        : base(data, index)
    {
    }

    public override string Display()
        => $"Digi Sound #{Index} Size ({Size} bytes)";
}