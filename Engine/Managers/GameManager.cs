using SDL2;

namespace Engine.Managers;

public class GameManager
{
    private readonly IAssetManager _assetManager;
    private readonly IVideoManager _videoManager;
    private readonly IInputManager _inputManager;
    private readonly IMapManager _mapManager;
    private readonly GamePackDefinitionAsset _packDefinitions;

    public GameManager(IAssetManager assetManager, IVideoManager videoManager, IInputManager inputManager, IMapManager mapManager, GamePackDefinitionAsset packDefinitions)
    {
        _assetManager = assetManager;
        _videoManager = videoManager;
        _inputManager = inputManager;
        _mapManager = mapManager;
        _packDefinitions = packDefinitions;
    }
    
    public void Start()
    {
        _videoManager.Initialize();
        
        var sceneManager = new SceneManager(_videoManager, _inputManager, _mapManager);
        sceneManager.LoadScene(_packDefinitions.StartingScene);
        
        // GameLoop
        // Main loop flag
        bool quit = false;

        // Main loop
        float elapsedMs = 0;
        while (!quit)
        {
            ulong start = SDL.SDL_GetPerformanceCounter();

            quit = _inputManager.IsQuitTriggered;
            
            // Handle physics
            sceneManager.OnPreUpdate(elapsedMs);
            
            //_videoManager.LimitFrameRate(70);
            ulong end = SDL.SDL_GetPerformanceCounter();
            elapsedMs = (end - start) / (float)SDL.SDL_GetPerformanceFrequency() * 1000.0f;

            var frameMs = 100.0f / 7.0f;
            if (elapsedMs < frameMs)
            {
                SDL.SDL_Delay((uint)(frameMs - elapsedMs));
            }
            
            // Handle rendering
            sceneManager.OnUpdate(elapsedMs);
            sceneManager.OnPostUpdate();
        }
        
        Quit();
    }

    private void Quit()
    {
        // TODO: Shut down everything else also (if it needs to be)
        _videoManager.Shutdown();
    }
}