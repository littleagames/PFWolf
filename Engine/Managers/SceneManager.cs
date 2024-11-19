namespace Engine.Managers;

public class SceneManager
{
    private readonly IVideoManager _videoManager;
    private readonly IInputManager _inputManager;
    private readonly IMapManager _mapManager;

    private dynamic? _contextData = null;

    public SceneManager(IVideoManager videoManager, IInputManager inputManager, IMapManager mapManager)
    {
        _videoManager = videoManager;
        _inputManager = inputManager;
        _mapManager = mapManager;
    }
    
    private Scene? _currentScene = null;
    public void LoadScene(string sceneName, dynamic? contextData = null)
    {
#if DEBUG
            _currentScene = new GameLoopScene();
#else
        var scriptAsset = (ScriptAsset?)_assetManager.FindAsset(AssetType.ScriptScene, sceneName);
        if (scriptAsset == null)
        {
            throw new InvalidDataException($"The specified scene \"{sceneName}\" does not exist.");
        }
        _currentScene = (Scene?)Activator.CreateInstance(scriptAsset.Script);
        if (_currentScene is null)
        {
            throw new InvalidDataException($"Could not properly build the script for scene \"{sceneName}\"");
        }
#endif

        if (_currentScene == null)
            return;
        
        _currentScene.StoreContextData(contextData);
        _currentScene.UpdateInputHandler(_inputManager.InputHandler); // TODO: Turn this into a component, remove this, and inputs are updated with the OnUpdate()
        _currentScene.OnStart();
        
        foreach (var component in _currentScene.Components.GetComponents())
        {
            ComponentStart(component);
        }
    }

    public void OnPreUpdate()
    {
        if (_currentScene == null)
            return;
        
        _currentScene.OnPreUpdate();
    }

    public void OnUpdate()
    {
        if (_currentScene == null)
            return;

        _currentScene.OnUpdate();
        
        foreach (var component in _currentScene.Components.GetComponents())
        {
            ComponentUpdate(component);
        }
        
        _videoManager.UpdateScreen();
        
        if (_currentScene.ChangeScene)
        {
            var nextScene = _currentScene.SceneQueuedToLoad;
            if (nextScene == null)
            {
                throw new Exception($"{_currentScene.GetType().Name} does not have a scene set to load.");
            }
                
            // TODO: Store the context data in a dictionary that is <sceneName, ContextData>
            // TODO: Unload scene (so the previous scene can run its OnDestroy (or OnSleep(), or OnAwake())
            LoadScene(nextScene, _currentScene.ContextData);
        }
        // TODO: Use _currentScene to check if scene name has changed, if so, end the scene, and create a new one with that name
        // If the new one doesn't exist, error, and don't destroy current scene or do anything
        // This will omit the scene loader
    }

    public void OnPostUpdate()
    {
        if (_currentScene == null)
            return;
        
        _currentScene.OnPostUpdate();
    }

    public void UnloadScene(string sceneName)
    {
        _currentScene?.OnDestroy();
    }
    
    private void ComponentStart(Component component)
    {
        component.OnStart();
        foreach (var innerComponent in component.Children.GetComponents())
        {
            ComponentStart(innerComponent);
        }
        // if (component is InputComponent)
        //     _inputManager.Start((InputComponent)component);
        
        // if (component is MapComponent)
        //     _mapManager.Start((MapComponent)component);
        
        // if (component is RenderComponent)
        //     _videoManager.Start((RenderComponent)component);
        //
    }

    private void ComponentUpdate(Component component)
    {
        component.OnUpdate();
        
        // if (component is InputComponent)
        //     _inputManager.Update((InputComponent)component);

        if (component is MapComponent)
            _mapManager.Update((MapComponent)component);
        
        if (component is RenderComponent)
            _videoManager.Update((RenderComponent)component);
        
        
        foreach (var innerComponent in component.Children.GetComponents())
        {
            ComponentUpdate(innerComponent);
        }
    }
}