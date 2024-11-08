using Engine.Scenes;
using LittleAGames.PFWolf.SDK.Components;
using Timer = LittleAGames.PFWolf.SDK.Components.Timer;

namespace Engine.Managers;

public class SceneManager
{
    private readonly IVideoManager _videoManager;

    private List<string> _scenes;
    
    public SceneManager(IVideoManager videoManager)
    {
        _videoManager = videoManager;
        _scenes = new List<string> { "SignonScene", "PG13Scene" };
    }
    
    private Scene? _currentScene = null;
    // TODO: Store all "Scene" objects here
    public void LoadScene(string sceneName)
    {
        if (sceneName == "SignonScene")
        {
            _currentScene = new SignonScene();
        }
        else if (sceneName == "Pg13Scene")
        {
            _currentScene = new PG13Scene();
        }
        else if (sceneName == "TitleScene")
        {
            _currentScene = new TitleScene();
        }
        else if (sceneName == "CreditsScene")
        {
            _currentScene = new CreditsScene();
        }
        else if (sceneName == "MainMenuScene")
        {
            _currentScene = new MainMenuScene();
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