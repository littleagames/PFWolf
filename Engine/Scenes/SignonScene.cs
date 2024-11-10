using LittleAGames.PFWolf.SDK;
using LittleAGames.PFWolf.SDK.Components;

[PfWolfScript("wolf3d:SignonScene")]
public class SignonScene : Scene
{
    private readonly PfTimer _pfTimer = new();
    private readonly Fader _fadeOutFader = Fader.Create(0.0f, 1.0f, 0x00, 0x00, 0x00, 20);
    
    public SignonScene()
    {
    }

    public override void OnStart()
    {
        Components.Add(Graphic.Create("wolf3d-signon", 0, 0));
        Components.Add(_pfTimer);
        Components.Add(_fadeOutFader);
        
        _pfTimer.Start();
    }

    public override void OnUpdate()
    {
        // TODO: Load information
        // "Press Any Key"
         if (_pfTimer.GetTime() > 210 /*|| Inputs.AnyKeyPressed*/)
         {
             _pfTimer.Stop();
             if (!_fadeOutFader.IsFading)
                _fadeOutFader.BeginFade();

             if (_fadeOutFader.IsComplete)
             {
                // LoadScene("wolf3d:MainMenuScene");
                 LoadScene("wolf3d:Pg13Scene");
             }
         }
    }
}