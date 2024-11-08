namespace LittleAGames.PFWolf.SDK.Components;

public class Timer : Component
{
    private long _timeInTics;
    private bool _paused;
    
    public Timer()
    {
        _timeInTics = 0;
        _paused = true;
    }

    public bool IsRunning => !_paused;

    /// <summary>
    /// Begins the timer
    /// </summary>
    public void Start()
    {
        _paused = false;
    }
    
    public override void OnUpdate()
    {
        if (!_paused)
            _timeInTics++;
    }

    public long GetTime() => _timeInTics;

    /// <summary>
    /// Stops the timer
    /// </summary>
    public void Stop()
    {
        _paused = true;
    }
}