namespace Engine.Managers;

public class GameManager
{
    private readonly AssetManager _assetManager;
    // private static volatile GameManager? _instance = null;
    // private static object syncRoot = new object();

    public GameManager(AssetManager assetManager)
    {
        _assetManager = assetManager;
    }
    
    // public static GameManager Instance
    // {
    //     get
    //     {
    //         if (_instance == null)
    //         {
    //             // only create a new instance if one doesn't already exist.
    //             lock (syncRoot)
    //             {
    //                 // use this lock to ensure that only one thread can access
    //                 // this block of code at once.
    //                 if (_instance == null)
    //                 {
    //                     _instance = new GameManager();
    //                 }
    //             }
    //         }
    //
    //         return _instance;
    //     }
    // }
    
    public void Start()
    {
        var videoMgr = VideoLayerManager.Instance;
        videoMgr.Start();
        
        // GameLoop
        var i = 0;
        do
        {
            videoMgr.DrawBackground((byte)((i/1000)%255));
            Console.WriteLine("Loop");
            i++;
            // TODO: Input manager to press keys
        } while (true);

        Quit();
    }

    private void Quit()
    {
        VideoLayerManager.Instance.Start();
    }
}