namespace Engine.Managers;

public class SceneManager
{
    private readonly IAssetManager _assetManager;
    private readonly IVideoManager _videoManager;
    private readonly IInputManager _inputManager;
    private readonly IMapManager _mapManager;

    private dynamic? _contextData = null;

    public SceneManager(
        IAssetManager assetManager,
        IVideoManager videoManager,
        IInputManager inputManager,
        IMapManager mapManager)
    {
        _assetManager = assetManager;
        _videoManager = videoManager;
        _inputManager = inputManager;
        _mapManager = mapManager;
    }
    
    private Scene? _currentScene = null;
    public void LoadScene(string sceneName, dynamic? contextData = null)
    {
// #if DEBUG
//             _currentScene = new GameLoopScene();
// #else
        var scriptAsset = _assetManager.FindAsset<ScriptAsset>(AssetType.ScriptScene, sceneName);
        if (scriptAsset == null)
        {
            throw new InvalidDataException($"The specified scene \"{sceneName}\" does not exist.");
        }
        _currentScene = (Scene?)Activator.CreateInstance(scriptAsset.Script);
        if (_currentScene is null)
        {
            throw new InvalidDataException($"Could not properly build the script for scene \"{sceneName}\"");
        }
//#endif

        if (_currentScene == null)
            return;
        
        _currentScene.StoreContextData(contextData);
        _currentScene.OnStart();
        foreach (var component in _currentScene.Components.GetComponents())
        {
            ComponentStart(component);
        }
    }

    public void OnPreUpdate(float deltaTime)
    {
        if (_currentScene == null)
            return;
        
        _currentScene.OnPreUpdate(deltaTime);
        foreach (var component in _currentScene.Components.GetComponents())
        {
            ComponentPreUpdate(component);
        }
    }

    public void OnUpdate(float deltaTime)
    {
        if (_currentScene == null)
            return;

        _currentScene.OnUpdate(deltaTime);
        
        foreach (var component in _currentScene.Components.GetComponents())
        {
            ComponentUpdate(component, deltaTime);
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
        
        if (component is InputComponent input)
            _inputManager.Initialize(input);
        
        if (component is MapComponent map)
            _mapManager.Initialize(map);
        
        // if (component is RenderComponent)
        //     _videoManager.Start((RenderComponent)component);
        //
    }

    private void ComponentPreUpdate(Component component)
    {
        component.OnPreUpdate();
        if (component is InputComponent input)
            _inputManager.Update(input);
        
        foreach (var innerComponent in component.Children.GetComponents())
        {
            ComponentPreUpdate(innerComponent);
        }
    }
    
    private void ComponentUpdate(Component component, float deltaTime)
    {
        component.OnUpdate();
        component.OnUpdate(deltaTime);
        
        switch (component)
        {
            case InputComponent input:
                _inputManager.Update(input);
                break;
            case MapComponent map:
                _mapManager.Update(map);
                break;
            case RenderComponent render:
                if (render.Hidden)
                    return;
                
                _videoManager.Update(render);
                break;
        }

        foreach (var innerComponent in component.Children.GetComponents())
        {
            ComponentUpdate(innerComponent, deltaTime);
        }
    }
}