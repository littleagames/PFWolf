using SDL2;

namespace Engine.Managers;

public class GameManager
{
    private readonly IAssetManager _assetManager;
    private readonly IVideoManager _videoManager;
    private readonly GameConfiguration _gameConfig;

    public GameManager(IAssetManager assetManager, IVideoManager videoManager, GameConfiguration gameConfig)
    {
        _assetManager = assetManager;
        _videoManager = videoManager;
        _gameConfig = gameConfig;
    }
    
    public void Start()
    {
        _videoManager.Initialize();
        
        var sceneManager = new SceneManager(_videoManager, _assetManager);
        sceneManager.LoadScene(_gameConfig.StartingScene);
        
        // GameLoop
        // Main loop flag
        bool quit = false;
        SDL.SDL_Event e;

        // Main loop
        while (!quit)
        {
            ulong start = SDL.SDL_GetPerformanceCounter();

            // Handle events on the queue
            while (SDL.SDL_PollEvent(out e) != 0)
            {
                // User requests quit
                if (e.type == SDL.SDL_EventType.SDL_QUIT)
                {
                    quit = true;
                }
            } 
            sceneManager.OnUpdate();
            ulong end = SDL.SDL_GetPerformanceCounter();
            float elapsedMS = (end - start) / (float)SDL.SDL_GetPerformanceFrequency() * 1000.0f;
            
            float frameMs = 14.28571428571429f;
            if (elapsedMS < frameMs)
            {
                SDL.SDL_Delay((uint)(frameMs - elapsedMS));
            }
        }
        
        Quit();
    }

    private void Quit()
    {
        // TODO: Shut down everything else also (if it needs to be)
        _videoManager.Shutdown();
    }
}