
namespace Engine.Managers;

internal class MenuManager
{
    private static volatile MenuManager? _instance = null;
    private static object syncRoot = new object();

    private ListMenuDescriptor _mainMenu = new();

    private MenuManager()
    {
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

    public void Init()
    {
        MenuDefManager.Instance.ParseMenuDefs();
        _mainMenu = SetMenu(MenuName.FromString("MainMenu"));
        //CreateMenus();
        /*
	if (!batchrun) Printf("M_Init: Init menus.\n");
	SetDefaultMenuColors();
	M_Init();
	M_CreateGameMenus();*/
    }

    public ListMenuDescriptor SetMenu(MenuName menuName)
    {
        var menuDefs = MenuDefManager.Instance;
        return menuDefs.GetMenu(menuName);
        //int menuX = (int)menuDescriptor.XPosition;
        //int menuY = (int)menuDescriptor.YPosition;
        //int linespacing = menuDescriptor.LineSpacing;

        //const byte MENU_W = 178;
        //byte MENU_H = (byte)(linespacing * 10 + 6);
        //menu.SetCursor(menuDescriptor.Selector, (int)menuDescriptor.SelectorOffsetX, (int)menuDescriptor.SelectorOffsetY);
        //menu.SetBackgroundColor(BORDCOLOR);

        //foreach (var md in menuDescriptor.Items)
        //{
        //    menu.AddItem()
        //}
        //menu.AddItem(new MenuGraphic(112, 184, "menus/mouselback")); // bottom centered (need tools to do that)
        //menu.AddItem(new MenuWindow(menuX - 8, menuY - 3, MENU_W, MENU_H, WindowBackgroundColor, BORD2COLOR, Deactive));
        //menu.AddItem(new MenuStripe(10, STRIPE));
        //menu.AddItem(new MenuGraphic(84, 0, "menus/mainmenu"));
        //menu.AddItem(new MenuSwitcherItem(menuX, menuY + (linespacing * 0), "New Game", TextColor, Deactive, Highlight, _newGame));
        //menu.AddItem(new MenuSwitcherItem(menuX, menuY + (linespacing * 1), "Sound", TextColor, Deactive, Highlight, _soundMenu));
        //menu.AddItem(new MenuSwitcherItem(menuX, menuY + (linespacing * 2), "Control", TextColor, Deactive, Highlight, _controlMenu));
        //menu.AddItem(new MenuItem(menuX, menuY + (linespacing * 3), "Load Game", TextColor, Deactive, Highlight));
        //menu.AddItem(new MenuItem(menuX, menuY + (linespacing * 4), "Save Game", TextColor, Deactive, Highlight));
        //menu.AddItem(new MenuItem(menuX, menuY + (linespacing * 5), "Read This!", TextColor, Deactive, Highlight));
        //menu.AddItem(new MenuItem(menuX, menuY + (linespacing * 6), "View Scores", TextColor, Deactive, Highlight));
        //menu.AddItem(new MenuItem(menuX, menuY + (linespacing * 7), "Back to Demo", TextColor, Deactive, Highlight));
        //menu.AddItem(new MenuItem(menuX, menuY + (linespacing * 8), "Quit", TextColor, Deactive, Highlight, (which) => { SDL2.SDL.SDL_Quit(); return false; }));
    }

    public void CreateMenus()
    {
        //const byte MENU_X = 76;
        //const byte MENU_Y = 55;
        //const byte MENU_INDENT = 28;
        //const byte MENU_W = 178;
        //const byte MENU_H = 13 * 10 + 6;

       // _mainMenu = new Menu();
    }

    public void Start()
    {
        // Begins the menu handling

        // do/while loop handling inputs for menus

        var exit = false;

        _mainMenu.Draw(VideoLayerManager.Instance);
        VideoLayerManager.Instance.FadeIn(10);
        int which;

        var running = true;

        // Main loop for the program
        while (running)
        {
            // Check to see if there are any events and continue to do so until the queue is empty.
            while (SDL2.SDL.SDL_PollEvent(out SDL2.SDL.SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL2.SDL.SDL_EventType.SDL_QUIT:
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
