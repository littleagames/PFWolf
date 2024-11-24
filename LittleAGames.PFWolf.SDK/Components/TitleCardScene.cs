namespace LittleAGames.PFWolf.SDK.Components;

public class TitleCardScene : Scene
{
    private readonly string _waitScene;
    private readonly string? _cutToScene;
    private readonly bool _useFadeIn;
    private readonly bool _useFadeOut;
    private readonly int _timeOnScene;
    
    private readonly PfTimer _pfTimer = new();
    private readonly Fader _fadeInFader = Fader.Create(1.0f, 0.0f, 0x00, 0x00, 0x00, 20);
    private readonly Fader _fadeOutFader = Fader.Create(0.0f, 1.0f, 0x00, 0x00, 0x00, 20);

    private bool _skipSceneKeyPressed = false;
    
    public TitleCardScene(string waitScene, string cutToScene, bool useFadeIn, bool useFadeOut, int timeOnScene)
    {
        _waitScene = waitScene;
        _cutToScene = cutToScene;
        _useFadeIn = useFadeIn;
        _useFadeOut = useFadeOut;
        _timeOnScene = timeOnScene;
        
        Components.Add(_pfTimer);
        Components.Add(_fadeInFader);
        Components.Add(_fadeOutFader);
    }

    public override void OnUpdate(float deltaTime)
    {
        // TODO: This needs to be called OnUpdate, but anything overriding it will no longer call this
        // What is the better choice to call this AND that, without a base.OnUpdate?
        if (_useFadeIn)
        {
            if (!_fadeInFader.IsFading)
                _fadeInFader.BeginFade();

            if (!_fadeInFader.IsComplete)
                return;
        }

        if (Input.IsAnyKeyPressed && !_skipSceneKeyPressed)
            _skipSceneKeyPressed = true;
        
        // Start wait timer after fade in
        if (!_pfTimer.IsRunning)
            _pfTimer.Start();
        
        if (_pfTimer.GetTime() > _timeOnScene || _skipSceneKeyPressed) 
        {
            _pfTimer.Stop();

            if (_useFadeOut)
            {
                if (!_fadeOutFader.IsFading)
                    _fadeOutFader.BeginFade();

                if (_fadeOutFader.IsComplete)
                {
                    if (!string.IsNullOrWhiteSpace(_cutToScene))
                    {
                        LoadScene(_cutToScene);
                    }
                    else
                        LoadScene(_waitScene);
                }
            }
            else
            {
                LoadScene(_waitScene);
            }
        }
    }
}