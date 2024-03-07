namespace Engine.StronglyTypeIds;


[StronglyTypedId(generateJsonConverter: true, StronglyTypedIdBackingType.Int, StronglyTypedIdJsonConverter.SystemTextJson)]
public partial struct FontColor
{
    public static FontColor FromByte(byte value) => new FontColor(value);
    public static FontColor FromInt(int value) => new FontColor(value);
    public static FontColor FromString(string value) => FromByte(Convert.ToByte(value));

    public byte GetByte()
    {
        return (byte)Value;
    }
}