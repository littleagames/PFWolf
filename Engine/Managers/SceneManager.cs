using Engine.Scenes;
using LittleAGames.PFWolf.SDK.Components;

namespace Engine.Managers;

public class SceneManager
{
    private readonly VideoLayerManager _videoManager;

    public SceneManager(VideoLayerManager videoManager)
    {
        _videoManager = videoManager;
    }
    
    private Scene? _currentScene = null;
    // TODO: Store all "Scene" objects here
    public void LoadScene(string sceneName)
    {
        _currentScene = new SignonScene();
        // Unload all other scenes
        // TODO: Make "scene" current scene
        _currentScene?.OnStart();
    }

    public void OnUpdate()
    {
        if (_currentScene == null)
            return;
        
        // TODO: Translate scene and its components to video manager
        foreach (var component in _currentScene.Components.GetComponents())
        {
            component.OnUpdate();
            if (component.GetType().IsAssignableFrom(typeof(RenderComponent)))
                _videoManager.Draw(component);
        }
        _videoManager.UpdateScreen();
        
        _currentScene.OnUpdate();
    }

    public void UnloadScene(string sceneName)
    {
        _currentScene?.OnDestroy();
    }
}