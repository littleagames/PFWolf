// Initializes SDL.
using Engine;
using Engine.Managers;
using Engine.Managers.Graphics;

InitGame();

DemoLoop();

// Clean up the resources that were created.
VideoLayerManager.Instance.Shutdown();


// END

void InitGame() {
    VideoLayerManager.Instance.Start();

    // Do I need to set up the menus here?
    MenuManager.Instance.CreateMenus();

    // TODO: Joystick setup

    SignonScreen();

    GraphicsManager.Instance.LoadDataFiles();

    // TODO:
    // ID startups


    FinishSignon();
}

void SignonScreen()
{
    var vl = VideoLayerManager.Instance;
    vl.Initialize(fullscreen: false);
    vl.MemToScreen(Signon.SignOn, 320, 200, 0, 0);
    vl.UpdateScreen();
}

void FinishSignon()
{
    var vl = VideoLayerManager.Instance;
    vl.DrawRectangle(0, 189, 300, 11, 0x29);

    vl.DrawTextString(100, 190, "Press a Key", new FontName("font/smallfont"), new FontColor(14)); // TODO: Centered
    vl.UpdateScreen();

    vl.WaitVBL(70 * 3);

    vl.DrawRectangle(0, 189, 300, 11, 0x29);
    vl.DrawTextString(100, 190, "Working...", new FontName("font/smallfont"), new FontColor(10)); // TODO: Centered
    vl.UpdateScreen();

    // This should be the end of the "Signon" screen
    vl.FadeOut();    
}

void DemoLoop()
{
    //NonShareware();
    //PG13();
    var vl = VideoLayerManager.Instance;

    //while(true)
    {
        //while(!inputGiven /*!nowait*/)
        {
            //TitleScreen();
            //Credits();
            //HighScores();
            //PlayDemo()
        }

        vl.FadeOut();

        MenuManager.Instance.Start();
        // US_ControlPanel(0);
    }
}

void PG13()
{
    var vl = VideoLayerManager.Instance;
    // draw
    vl.DrawBackground(0x82); // background

    vl.DrawPic(216, 110, "pg13");
    vl.UpdateScreen();

    // OnStart
    vl.FadeIn();

    // ??? OnKeyPress, or in Update
    vl.WaitVBL(70 * 7);
    //IN_UserInput(TickBase * 7);

    // OnLeave
    vl.FadeOut();
}

void TitleScreen()
{
    var vl = VideoLayerManager.Instance;
    vl.DrawBackground(0x00);
    vl.DrawPic(0, 0, "title");
    vl.UpdateScreen();
    vl.FadeIn();
    vl.WaitVBL(70 * 7);
    vl.FadeOut();

    // TODO: On keypress
    //       Change "GameLoopState" from "IdleLoop" to "Menu"
}
void Credits()
{
    var vl = VideoLayerManager.Instance;

    vl.DrawBackground(0x00);
    vl.DrawPic(0, 0, "credits");
    vl.UpdateScreen();
    vl.FadeIn();
    vl.WaitVBL(70 * 7);
    vl.FadeOut();
}