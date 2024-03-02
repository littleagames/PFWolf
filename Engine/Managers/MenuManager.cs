using Engine.DataModels;
using SDL2;
using static SDL2.SDL;

namespace Engine.Managers;

internal class MenuManager
{
    private static volatile MenuManager? _instance = null;
    private static object syncRoot = new object();

    private Menu _mainMenu = null!;

    private MenuManager()
    {
        CreateMenus();
    }

    /// <summary>
    /// The instance of the singleton
    /// safe for multithreading
    /// </summary>
    public static MenuManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // only create a new instance if one doesn't already exist.
                lock (syncRoot)
                {
                    // use this lock to ensure that only one thread can access
                    // this block of code at once.
                    if (_instance == null)
                    {
                        _instance = new MenuManager();
                    }
                }
            }

            return _instance;
        }
    }

    public void CreateMenus()
    {
        const byte MENU_X = 76;
        const byte MENU_Y = 55;
        _mainMenu = new Menu();
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y, "New Game", null));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 1), "Sound", null));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 2), "Control", null));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 3), "Load Game", null));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 4), "Save Game", null));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 5), "Read This!", null));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 6), "View Scores", null));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 7), "Back to Demo", null));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 8), "Quit", null));
    }

    public void Start()
    {
        // Begins the menu handling

        // MainMenu.draw()
        // MenuFadeIn();

        // do/while loop handling inputs for menus

        var exit = false;

        _mainMenu.Draw();
        VideoLayerManager.Instance.FadeIn(10);
        int which;

        var running = true;

        // Main loop for the program
        while (running)
        {
            // Check to see if there are any events and continue to do so until the queue is empty.
            while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                }
            }
        }

            //do
            //{
            //    which = _mainMenu.Handle();
            //}
            //while (!exit);
    }
}
