using LittleAGames.PFWolf.SDK.Handlers;

namespace LittleAGames.PFWolf.SDK.Components;

public abstract class Scene : RunnableBase
{
    public bool ChangeScene { get; private set; } = false;
    public string? SceneQueuedToLoad { get; private set; } = null;

    public ComponentCollection Components { get; private set; } = new();

    public SceneContext? ContextData { get; private set; }
    
    public InputHandler Input { get; private set; } = new();
    //public RenderHandler Renderer { get; private set; } = new();

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

    protected void LoadScene(string sceneName, SceneContext? data = null)
    {
        StoreContextData(data);
        ChangeScene = true;
        SceneQueuedToLoad = sceneName;
        // TODO: Store data (overwrite whatever was there?)
    }

    public void StoreContextData(SceneContext? data)
    {
        if (data != null)
        {
            ContextData = data;
        }
    }

    public void UpdateInputHandler(InputHandler handler)
    {
        Input = handler;
    }

    protected string GetSceneName()
    {
        var attribute = Attribute.GetCustomAttributes(this.GetType(), typeof(PfWolfSceneAttribute))
            .Cast<PfWolfSceneAttribute>()
            .FirstOrDefault();
        
        return attribute?.ScriptName ?? GetType().Name;
    }
}