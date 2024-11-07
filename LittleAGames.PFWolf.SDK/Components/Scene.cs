namespace LittleAGames.PFWolf.SDK.Components;

public abstract class Scene : RunnableBase
{
    public Scene(string name)
    {
        _name = name;
    }

    private string _name;
    public bool _changeScene;
    public string _nextScene;

    public ComponentCollection Components { get; private set; } = new();

    public virtual void OnStart()
    {
        
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnDestroy()
    {
    }

    public void LoadScene(string sceneName)
    {
        _changeScene = true;
        _nextScene = sceneName;
    }
}