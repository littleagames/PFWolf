// Initializes SDL.
using Engine;
using Engine.Managers;

InitGame();

DemoLoop();


// START TEST
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

// Clean up the resources that were created.
VisualLayerManager.Instance.Shutdown();
SDL.SDL_Quit();

// END

void InitGame() {
    VisualLayerManager.Instance.Start();

    // TODO: Joystick setup

    SignonScreen();

    DataFileManager.Instance.LoadDataFiles();

    // TODO:
    // ID startups


    FinishSignon();
}

void SignonScreen()
{
    VisualLayerManager.Instance.Initialize(fullscreen: false);

    VisualLayerManager.Instance.MemToScreen(Signon.SignOn, 320, 200, 0, 0);

    VisualLayerManager.Instance.UpdateScreen();
}

void FinishSignon()
{
    VisualLayerManager.Instance.DrawRectangle(0, 189, 300, 11, 0x29);

    //US_CPrint("Press a key");
    VisualLayerManager.Instance.UpdateScreen();

    VisualLayerManager.Instance.WaitVBL(70 * 3);

    //US_CPrint("Working...");
    //ID_VL.VW_UpdateScreen();

    // This should be the end of the "Signon" screen
    VisualLayerManager.Instance.FadeOut();
    /*
     
    WindowX = 0;
    WindowW = 320;
    PrintY = 190;

    #ifndef JAPAN
    SETFONTCOLOR(14,4);

    #ifdef SPANISH
    US_CPrint ("Oprima una tecla");
    #else
    US_CPrint ("Press a key");
    #endif

    #endif

    VW_UpdateScreen();

    if (!param_nowait)
        IN_Ack ();

    #ifndef JAPAN
    VW_Bar (0,189,300,11,VL_GetPixel(0,0));

    PrintY = 190;
    SETFONTCOLOR(10,4);

    #ifdef SPANISH
    US_CPrint ("pensando...");
    #else
    US_CPrint ("Working...");
    #endif

    VW_UpdateScreen();
    #endif

    SETFONTCOLOR(0,15);
     */
}

void DemoLoop()
{
    PG13();
    TitleScreen();
}

void PG13()
{
    VisualLayerManager.Instance.DrawRectangle(0, 0, 320, 200, 0x82); // background

    VisualLayerManager.Instance.DrawPic(216, 110, "PG13");
    VisualLayerManager.Instance.UpdateScreen();

    VisualLayerManager.Instance.FadeIn();

    VisualLayerManager.Instance.WaitVBL(70 * 7);
    //IN_UserInput(TickBase * 7);

    VisualLayerManager.Instance.FadeOut();
}

void TitleScreen()
{
    VisualLayerManager.Instance.DrawRectangle(0, 0, 320, 200, 0x01); // background

    VisualLayerManager.Instance.DrawPic(0, 0, "TITLE");

    VisualLayerManager.Instance.UpdateScreen();

    VisualLayerManager.Instance.FadeIn();

    VisualLayerManager.Instance.WaitVBL(70 * 7);

    VisualLayerManager.Instance.FadeOut();
}