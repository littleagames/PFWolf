namespace LittleAGames.PFWolf.SDK.Models;

public class SceneContext : Dictionary<string, object>
{
    public T? GetProperty<T>(string propertyName)
    {
        if (TryGetValue(propertyName, out var value))
            return (T)value;
        
        return default;
    }
}