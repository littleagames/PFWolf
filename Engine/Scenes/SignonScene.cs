[PfWolfScene("wolf3d:SignonScene")]
public class SignonScene : Scene
{
    private readonly PfTimer _pfTimer = new();
    private readonly Fader _fadeOutFader = Fader.Create(0.0f, 1.0f, 0x00, 0x00, 0x00, 20);

    private bool _loadingConsoleData = true;
    
    public SignonScene()
    {
        Components.Add(Graphic.Create("wolf3d-signon", 0, 0));
        Components.Add(_pfTimer);
        Components.Add(_fadeOutFader);
    }

    public override void OnStart()
    {
        _pfTimer.Start();
    }

    public override void OnUpdate()
    {
        if (_loadingConsoleData && _pfTimer.GetTime() > 210)
        {
            _pfTimer.Reset();
            _loadingConsoleData = false;
        }

        if (!Input.IsAnyKeyPressed)
            return;

        if (!_fadeOutFader.IsFading)
                _fadeOutFader.BeginFade();

        if (_fadeOutFader.IsComplete)
        {
            LoadScene("wolf3d:Pg13Scene");
        }
    }
}