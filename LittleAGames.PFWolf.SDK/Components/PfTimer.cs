﻿namespace LittleAGames.PFWolf.SDK.Components;

public class PfTimer : Component
{
    private long _timeInTics;
    private bool _paused;
    
    public PfTimer()
    {
        _timeInTics = 0;
        _paused = true;
    }

    public bool IsRunning => !_paused;

    public long GetTime() => _timeInTics;

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

    /// <summary>
    /// Stops the timer
    /// </summary>
    public void Stop()
    {
        _paused = true;
    }

    public void Reset()
    {
        _timeInTics = 0;
    }
}