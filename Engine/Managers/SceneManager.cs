using Engine.Scenes;
using LittleAGames.PFWolf.SDK.Components;
using Timer = LittleAGames.PFWolf.SDK.Components.Timer;

namespace Engine.Managers;

public class SceneManager
{
    private readonly IVideoManager _videoManager;

    public SceneManager(IVideoManager videoManager)
    {
        _videoManager = videoManager;
    }
    
    private Scene? _currentScene = null;
    public void LoadScene(string sceneName)
    {
        // assetManager.Scripts(AssetType.ScriptScene, sceneName)
        if (sceneName == "wolf3d:SignonScene")
        {
            _currentScene = new SignonScene();
        }
        else if (sceneName == "wolf3d:Pg13Scene")
        {
            _currentScene = new PG13Scene();
        }
        else if (sceneName == "wolf3d:TitleScene")
        {
            _currentScene = new TitleScene();
        }
        else if (sceneName == "wolf3d:CreditsScene")
        {
            _currentScene = new CreditsScene();
        }
        else if (sceneName == "wolf3d:MainMenuScene")
        {
            _currentScene = new MainMenuScene();
        }
        else if (sceneName == "wolf3d:ViewScoresScene")
        {
            _currentScene = new ViewScoresScene();
        }

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
        if (_currentScene._changeScene)
        {
            var nextScene = _currentScene._nextScene;
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