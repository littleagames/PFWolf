using LittleAGames.PFWolf.SDK;
using LittleAGames.PFWolf.SDK.Components;

[PfWolfScript("wolf3d:MainMenuScene")]
public class MainMenuScene : Scene
{
    private readonly PfTimer _pfTimer = new();
    private readonly Fader _fadeInFader = Fader.Create(1.0f, 0.0f, 0x00, 0x00, 0x00, 20);
    private readonly Fader _fadeOutFader = Fader.Create(0.0f, 1.0f, 0x00, 0x00, 0x00, 20);
    private readonly Menu _menu;
    
    public MainMenuScene()
    {
        var menuX = 76;
        var menuY = 55;
        var menuIndent = 24;
        var lineSpacing = 13;
       
        _menu = Menu.Create(menuX, menuY, menuIndent, lineSpacing);
        _menu.AddMenuItem("New Game", activeState: 1, NewGame);
        _menu.AddMenuItem("Sound", activeState: 1, NoOp);
        _menu.AddMenuItem("Control", activeState: 1, NoOp);
        _menu.AddMenuItem("Load Game", activeState: 1, NoOp);
        _menu.AddMenuItem("Save Game", activeState: 0, NoOp);
        _menu.AddMenuItem("Change View", activeState: 1, NoOp);
        _menu.AddMenuItem("Read This!", activeState: 2, NoOp);
        _menu.AddMenuItem("Back to Demo", activeState: 1, NoOp);
        _menu.AddMenuItem("Quit", activeState: 1, NoOp);
    }

    public void NewGame()
    {
    }

    public void NoOp()
    {
    }

    public override void OnStart()
    {
        Components.Add(Background.Create(0x29));
        Components.Add(Graphic.Create("c_mouselback", 112, 184));
        Components.Add(Stripe.Create(10, 0x2c));
        Components.Add(Graphic.Create("c_options", 84, 0));
        Components.Add(Wolf3DBorderedWindow.Create(68, 52, 178, 13*9+6));
        
        Components.Add(Graphic.Create("C_Cursor1", 72, 55+13*0));
        Components.Add(_menu);
        
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
    }

    // protected override void OnDestroy()
    // {
    //     throw new NotImplementedException();
    // }
}