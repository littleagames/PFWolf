using LittleAGames.PFWolf.SDK;
using LittleAGames.PFWolf.SDK.Components;
using LittleAGames.PFWolf.SDK.Handlers;

[PfWolfScript("wolf3d:MainMenuScene")]
public class MainMenuScene : MenuScene
{
    private readonly PfTimer _pfTimer = new();
    private readonly Fader _fadeInFader = Fader.Create(1.0f, 0.0f, 0x00, 0x00, 0x00, 20);
    private readonly Fader _fadeOutFader = Fader.Create(0.0f, 1.0f, 0x00, 0x00, 0x00, 20);
    
    public MainMenuScene()
    {
        var menuX = 76;
        var menuY = 55;
        var menuIndent = 24;
        var lineSpacing = 13;
       
        Menu = Menu.Create(menuX, menuY, menuIndent, lineSpacing);
        Menu.AddMenuItem("New Game", ActiveState.Active, NewGame);
        Menu.AddMenuItem("Sound", ActiveState.Active, NoOp);
        Menu.AddMenuItem("Control", ActiveState.Active, NoOp);
        Menu.AddMenuItem("Load Game", ActiveState.Active, NoOp);
        Menu.AddMenuItem("Save Game", ActiveState.Disabled, NoOp);
        Menu.AddMenuItem("Change View", ActiveState.Active, NoOp);
        Menu.AddMenuItem("Read This!", ActiveState.Special, NoOp);
        Menu.AddMenuItem("Back to Demo", ActiveState.Active, NoOp);
        Menu.AddMenuItem("Quit", ActiveState.Active, NoOp);
    }

    public void NewGame()
    {
        // TODO: Store data in the scene
        /*
         * { ParentMenu: { Scene: "wolf3d:MainMenuScene", MenuIndex: CurrentIndex } }
         * 
         */
        LoadScene("wolf3d:EpisodeSelectScene"/*, data*/);
        return;
    }

    public void NoOp()
    {
        return;
    }

    public override void OnStart()
    {
        Components.Add(Background.Create(0x29));
        Components.Add(Graphic.Create("c_mouselback", 112, 184));
        Components.Add(Stripe.Create(10, 0x2c));
        Components.Add(Graphic.Create("c_options", 84, 0));
        Components.Add(Wolf3DBorderedWindow.Create(68, 52, 178, 13*9+6));
        Components.Add(Menu);
        
        Components.Add(Graphic.Create("C_Cursor1", 72, 55+13*0));
        
        Components.Add(_pfTimer);
        Components.Add(_fadeInFader);
        Components.Add(_fadeOutFader);
    }

    public override void OnUpdate()
    {
        if (!_fadeInFader.IsFading)
            _fadeInFader.BeginFade();

        if (!_fadeInFader.IsComplete)
            return;

        // TODO: This should be in a HandleMenu call
        if (Input.IsKeyDown(Keys.Down))
        {
            Menu.MoveDown();
            Input.ClearKeysDown();
            return;
        }
        
        if (Input.IsKeyDown(Keys.Up))
        {
            Menu.MoveUp();
            Input.ClearKeysDown();
            return;
        }
        
        if (Input.IsKeyDown(Keys.Return))
        {
            Menu.PerformAction(); // GoToMenu(menu.ActionMenu)
            Input.ClearKeysDown();
            return;
        }
        
        if (Input.IsKeyDown(Keys.Escape))
        {
            //Menu.PerformExitAction(Menu.ParentMenu); // TODO: Action would either be a "GoToMenu(menu.ParentMenu)" or "ConfirmDialog"
            Input.ClearKeysDown();
            return;
        }
    }

    // protected override void OnDestroy()
    // {
    //     throw new NotImplementedException();
    // }
}