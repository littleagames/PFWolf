namespace LittleAGames.PFWolf.SDK.Components;

public class SceneLoader : Component
{
    public SceneLoader()
    {
    }

    public void LoadScene(string sceneName)
    {
        _sceneName = sceneName;
    }

    private string? _sceneName = null;
}