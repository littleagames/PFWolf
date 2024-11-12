[PfWolfScene("wolf3d:SignonScene")]
public class SignonScene : Scene
{
    private readonly PfTimer _pfTimer = new();
    private readonly Fader _fadeOutFader = Fader.Create(0.0f, 1.0f, 0x00, 0x00, 0x00, 20);
    private Text PressAnyKeyText;
    private Text WorkingText;

    private bool _loadingConsoleData = true;
    private bool _waitingForKeyPress = false;
    
    public SignonScene()
    {
        PressAnyKeyText = Text.Create("Press Any Key", 20, 190, "SmallFont", 14);
        PressAnyKeyText.Hidden = true;
        WorkingText = Text.Create("Working...", 20, 190, "SmallFont", 10);
        WorkingText.Hidden = true;
        
        Components.Add(Graphic.Create("wolf3d-signon", 0, 0));
        Components.Add(_pfTimer);
        Components.Add(_fadeOutFader);

        Components.Add(PressAnyKeyText);
        Components.Add(WorkingText);
    }

    public override void OnStart()
    {
        _pfTimer.Start();
    }

    public override void OnUpdate()
    {
        if (_loadingConsoleData && _pfTimer.GetTime() <= 70)
            return;
        
        if (_pfTimer.GetTime() > 70)
        {
            _pfTimer.Reset();
            _loadingConsoleData = false;
            PressAnyKeyText.Hidden = false;
            _waitingForKeyPress = true;
        }

        if (!Input.IsAnyKeyPressed && _waitingForKeyPress)
            return;

        _waitingForKeyPress = false;
        PressAnyKeyText.Hidden = true;
        WorkingText.Hidden = false;

        if (!_fadeOutFader.IsFading)
                _fadeOutFader.BeginFade();

        if (_fadeOutFader.IsComplete)
        {
            LoadScene("wolf3d:Pg13Scene");
        }
    }
}