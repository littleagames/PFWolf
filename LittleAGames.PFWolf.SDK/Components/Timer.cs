namespace LittleAGames.PFWolf.SDK.Components;

public class Timer : Component
{
    private long _timeinTics;
    private bool _paused;
    public Timer()
    {
        _timeinTics = 0;
        _paused = true;
    }

    public void Update()
    {
        if (!_paused)
            _timeinTics++;
    }

    public long GetTime() => _timeinTics;

    public void Stop()
    {
        _paused = true;
    }
}