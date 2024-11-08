namespace LittleAGames.PFWolf.SDK.Components;

public class Fader : Timer
{
    public float CurrentOpacity { get; private set; }
    public float OpacityBegin { get; }
    public float OpacityFinish { get; }
    
    public byte Red { get; }
    public byte Green { get; }
    public byte Blue { get; }
    
    public short Duration { get; }

    public bool IsFading => GetTime() > 0 && !IsComplete;
    public bool IsComplete => (Duration - GetTime()) <= 0;

    public static Fader Create(float opacityBegin, float opacityFinish, byte red, byte green, byte blue, short duration)
        => new(opacityBegin, opacityFinish, red, green, blue, duration);
    
    private Fader(float opacityBegin, float opacityFinish, byte red, byte green, byte blue, short duration)
    {
        if (opacityBegin < 0.0f || opacityBegin > 1.0)
            throw new ArgumentOutOfRangeException(nameof(opacityBegin), opacityBegin, "opacityBegin must be between 0.0 and 1.0");
        
        OpacityBegin = opacityBegin;
        
        if (opacityFinish < 0.0f || opacityFinish > 1.0)
            throw new ArgumentOutOfRangeException(nameof(opacityFinish), opacityFinish, "opacityFinish must be between 0.0 and 1.0");
        
        OpacityFinish = opacityFinish;

        Red = red;
        Green = green;
        Blue = blue;
        
        Duration = duration;
    }

    public override void OnStart()
    {
        CurrentOpacity = OpacityBegin;
    }

    public void BeginFade()
    {
        if (IsFading)
            return;
        // Start the timer
        base.Start();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var currentTime = GetTime();
        if (currentTime > Duration)
            currentTime = Duration;
        
        var timeLeft = (Duration - currentTime);
        if (timeLeft <= 0)
        {
            // Prevent any precision errors
            // Just set it flat to finish value
            CurrentOpacity = OpacityFinish;
            base.Stop();
            return;
        }

        var percentCompleted = (float)currentTime / Duration;
        var rangeOfOpacity = OpacityFinish - OpacityBegin;
        var changeFromBeginning = rangeOfOpacity * percentCompleted;

        CurrentOpacity = OpacityBegin + changeFromBeginning;
    }
}