using LittleAGames.PFWolf.SDK.Components;
using Timer = LittleAGames.PFWolf.SDK.Components.Timer;

namespace Engine.Scenes;

public class ViewScoresScene : Scene
{
    private readonly Timer _timer = new();
    private readonly Fader _fadeInFader = Fader.Create(1.0f, 0.0f, 0x00, 0x00, 0x00, 20);
    private readonly Fader _fadeOutFader = Fader.Create(0.0f, 1.0f, 0x00, 0x00, 0x00, 20);
    
    public ViewScoresScene()
        : base("wolf3d:ViewScoresScene")
    {
    }

    public override void OnStart()
    {
        Components.Add(Background.Create(0x29));
        Components.Add(Stripe.Create(10, 0x2c));
        Components.Add(Graphic.Create("highscores", 48, 0));
        Components.Add(Graphic.Create("c_name", 24, 68));
        Components.Add(Graphic.Create("c_level", 160, 68));
        Components.Add(Graphic.Create("c_score", 28*8, 68));
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
        
        // Start wait timer after fade in
        if (!_timer.IsRunning)
            _timer.Start();
        
        if (_timer.GetTime() > 300) 
        {
            _timer.Stop();
            
            if (!_fadeOutFader.IsFading)
                _fadeOutFader.BeginFade();
            
            if (_fadeOutFader.IsComplete)
            {
                LoadScene("wolf3d:MainMenuScene");
            }
        }
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