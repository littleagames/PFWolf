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

    public override void OnStart()
    {
        _paused = false;
    }
    
    public void Pause() => _paused = true;
    public void Resume() => _paused = false;
    
    public override void OnUpdate()
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