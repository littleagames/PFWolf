namespace Engine.StronglyTypeIds;


[StronglyTypedId(generateJsonConverter: true, StronglyTypedIdBackingType.Int, StronglyTypedIdJsonConverter.SystemTextJson)]
public partial struct FontColor
{
    public FontColor(byte color) { Value = color; }
}