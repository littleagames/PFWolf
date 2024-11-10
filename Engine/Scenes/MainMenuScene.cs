using LittleAGames.PFWolf.SDK;
using LittleAGames.PFWolf.SDK.Components;
using Timer = LittleAGames.PFWolf.SDK.Components.Timer;

[PfWolfScript("wolf3d:MainMenuScene")]
public class MainMenuScene : Scene
{
    private readonly Timer _timer = new();
    private readonly Fader _fadeInFader = Fader.Create(1.0f, 0.0f, 0x00, 0x00, 0x00, 20);
    private readonly Fader _fadeOutFader = Fader.Create(0.0f, 1.0f, 0x00, 0x00, 0x00, 20);
    
    public MainMenuScene()
    {
    }

    public override void OnStart()
    {
        Components.Add(Background.Create(0x29));
        Components.Add(Graphic.Create("c_mouselback", 112, 184));
        Components.Add(Stripe.Create(10, 0x2c));
        Components.Add(Graphic.Create("c_options", 84, 0));
        Components.Add(Wolf3DBorderedWindow.Create(68, 55, 178, 13*9+6));
        Components.Add(_timer);
        Components.Add(_fadeInFader);
        Components.Add(_fadeOutFader);
    }

    public override void OnUpdate()
    {
        if (!_fadeInFader.IsFading)
            _fadeInFader.BeginFade();

        if (!_fadeInFader.IsComplete)
            return;
        
        // // Start wait timer after fade in
        // if (!_timer.IsRunning)
        //     _timer.Start();
        //
        // if (_timer.GetTime() > 300) 
        // {
        //     _timer.Stop();
        //     
        //     if (!_fadeOutFader.IsFading)
        //         _fadeOutFader.BeginFade();
        //     
        //     if (_fadeOutFader.IsComplete)
        //     {
        //         LoadScene("CreditsScene");
        //     }
        // }
        // else if (Inputs.AnyKeyPressed)
        // {
        //     LoadScene("MainMenu");
        // }
    }

    // protected override void OnEnd()
    // {
    //     throw new NotImplementedException();
    // }
}