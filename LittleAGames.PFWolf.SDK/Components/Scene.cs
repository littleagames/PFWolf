namespace LittleAGames.PFWolf.SDK.Components;

public abstract class Scene : RunnableBase
{
    public bool ChangeScene { get; private set; } = false;
    public string? SceneQueuedToLoad { get; private set; } = null;

    public ComponentCollection Components { get; private set; } = new();

    public virtual void OnStart()
    {
        
    }

    public virtual void OnPreUpdate()
    {
    }
    
    public virtual void OnUpdate()
    {
    }

    public virtual void OnPostUpdate()
    {
    }
    
    public virtual void OnDestroy()
    {
    }

    protected void LoadScene(string sceneName)
    {
        ChangeScene = true;
        SceneQueuedToLoad = sceneName;
    }
}