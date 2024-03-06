namespace Engine.StronglyTypeIds;


[StronglyTypedId(generateJsonConverter: true, StronglyTypedIdBackingType.String, StronglyTypedIdJsonConverter.SystemTextJson)]
public partial struct FontName
{
    public static FontName FromString(string value) => new FontName(value);
}