namespace Engine.Managers;

public class SceneManager
{
    private readonly IVideoManager _videoManager;
    private readonly IAssetManager _assetManager;
    private readonly IInputManager _inputManager;
    private readonly IMapManager _mapManager;

    private dynamic? _contextData = null;

    public SceneManager(IVideoManager videoManager, IAssetManager assetManager, IInputManager inputManager, IMapManager mapManager)
    {
        _videoManager = videoManager;
        _assetManager = assetManager;
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
        _currentScene.UpdateInputHandler(_inputManager.InputHandler);
        _currentScene.OnStart();
        
        foreach (var component in _currentScene.Components.GetComponents())
        {
            if (component is Map)
            {
                var map = (Map)component;
                _mapManager.BuildMap(map /*, mapDefinitions*/);
            }
            
            component.OnStart();
            
            foreach (var innerComponent in component.Children.GetComponents())
            {
                innerComponent.OnStart();
            }
        }
    }

    public void OnPreUpdate()
    {
        if (_currentScene == null)
            return;
        
        // foreach (var component in _currentScene.Components.GetComponents())
        // {
        //     ComponentUpdate(component);
        //     
        //     foreach (var innerComponent in component.Children.GetComponents())
        //     {
        //         ComponentUpdate(innerComponent);
        //     }
        // }
        
        _currentScene.OnPreUpdate();
    }

    public void OnUpdate()
    {
        if (_currentScene == null)
            return;

        foreach (var component in _currentScene.Components.GetComponents())
        {
            ComponentUpdate(component);
            
            foreach (var innerComponent in component.Children.GetComponents())
            {
                ComponentUpdate(innerComponent);
            }
        }
        
        _videoManager.UpdateScreen();
        
        _currentScene.OnUpdate();
        if (_currentScene.ChangeScene)
        {
            var nextScene = _currentScene.SceneQueuedToLoad;
            if (nextScene == null)
            {
                throw new Exception($"{_currentScene.GetType().Name} does not have a scene set to load.");
            }
                
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

    private void ComponentUpdate(Component component)
    {
        component.OnUpdate();
        
        
        // TODO: If rendercomponent only
        // TODO: Check here if any non-rendercomponents are being passed in
        _videoManager.Update(component);
    }
    
    private void StoreContextData(dynamic? data)
    {
        if (data != null)
        {
            _contextData = data;
        }
    }
}