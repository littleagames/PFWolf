[PfWolfScene("wolf3d:InitializeGameScene")]
public class InitializeGameScene : Scene
{
    private Fader _fadeInFader { get; }
    private Fader _fadeOutFader { get; }
    public InitializeGameScene()
    {
        _fadeInFader = Fader.Create(1.0f, 0.0f, 0x00, 0x00, 0x00, 20);
        _fadeOutFader = Fader.Create(0.0f, 1.0f, 0x00, 0x00, 0x00, 20);
        
        Components.Add(Background.Create(0x7f));
        Components.Add(Graphic.Create("getpsyched",((320-224)/16)*8, (200-(40+48))/2));
        
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
}