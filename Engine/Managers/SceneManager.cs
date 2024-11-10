using Engine.Scenes;
using LittleAGames.PFWolf.SDK.Assets;
using LittleAGames.PFWolf.SDK.Components;
using Timer = LittleAGames.PFWolf.SDK.Components.Timer;

namespace Engine.Managers;

public class SceneManager
{
    private readonly IVideoManager _videoManager;
    private readonly IAssetManager _assetManager;

    public SceneManager(IVideoManager videoManager, IAssetManager assetManager)
    {
        _videoManager = videoManager;
        _assetManager = assetManager;
    }
    
    private Scene? _currentScene = null;
    public void LoadScene(string sceneName)
    {
#if FALSE // DEBUG
        _currentScene = new PG13Scene();
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

        // Unload all other scenes
        // TODO: Make "scene" current scene
        _currentScene?.OnStart();
    }

    public void OnUpdate()
    {
        if (_currentScene == null)
            return;
        
        //_currentScene.OnPreUpdate();
        
        foreach (var component in _currentScene.Components.GetComponents())
        {
            component.OnUpdate();
            _videoManager.Update(component);
        }
        
        _videoManager.UpdateScreen();
        
        _currentScene.OnUpdate();
        if (_currentScene.ChangeScene)
        {
            var nextScene = _currentScene.SceneQueuedToLoad;
            // TODO: VerifyScene(nextScene); // Make sure scene exists before destroying and loading
            _currentScene?.OnDestroy();
            LoadScene(nextScene);
        }
        // TODO: Use _currentScene to check if scene name has changed, if so, end the scene, and create a new one with that name
        // If the new one doesn't exist, error, and don't destroy current scene or do anything
        // This will omit the scene loader
    }

    public void UnloadScene(string sceneName)
    {
        _currentScene?.OnDestroy();
    }
}