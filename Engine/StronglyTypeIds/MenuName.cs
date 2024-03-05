namespace Engine.StronglyTypeIds;

[StronglyTypedId(generateJsonConverter: true, StronglyTypedIdBackingType.String, StronglyTypedIdJsonConverter.SystemTextJson)]
public partial struct MenuName
{
    public static MenuName FromString(string value) => !string.IsNullOrWhiteSpace(value) ? new MenuName(value) : Empty;
}