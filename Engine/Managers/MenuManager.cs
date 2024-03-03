using Engine.DataModels;
using SDL2;
using static SDL2.SDL;

namespace Engine.Managers;

internal class MenuManager
{
    private static volatile MenuManager? _instance = null;
    private static object syncRoot = new object();

    private Menu _mainMenu = null!;

    private readonly FontColor TextColor = new FontColor(0x17);

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
        const byte MENU_W = 178;
        const byte MENU_H = 13 * 10 + 6;

        FontColor BORDCOLOR = new FontColor(0x29);
        const byte BORD2COLOR = 0x23;
        const byte DEACTIVE = 0x2b;
        FontColor BKGDCOLOR = new FontColor(0x2d);
        FontColor STRIPE = new FontColor(0x2c);

        _mainMenu = new Menu();
        _mainMenu.SetBackgroundColor(BORDCOLOR);
        _mainMenu.AddItem(new MenuGraphic(112, 184, "menus/mouselback")); // bottom centered (need tools to do that)
        _mainMenu.AddItem(new MenuWindow(MENU_X - 8, MENU_Y - 3, MENU_W, MENU_H, BKGDCOLOR, BORD2COLOR, DEACTIVE));
        _mainMenu.AddItem(new MenuStripe(10, STRIPE));
        _mainMenu.AddItem(new MenuGraphic(84, 0, "menus/mainmenu"));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y, "New Game", TextColor));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 1), "Sound", TextColor));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 2), "Control", TextColor));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 3), "Load Game", TextColor));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 4), "Save Game", TextColor));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 5), "Read This!", TextColor));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 6), "View Scores", TextColor));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 7), "Back to Demo", TextColor));
        _mainMenu.AddItem(new MenuSwitcherItem(MENU_X, MENU_Y + (13 * 8), "Quit", TextColor));
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
