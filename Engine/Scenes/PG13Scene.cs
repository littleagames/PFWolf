using LittleAGames.PFWolf.SDK.Components;
using Timer = LittleAGames.PFWolf.SDK.Components.Timer;

namespace Engine.Scenes;

public class PG13Scene : Scene
{
    private readonly Timer _timer = new();
    
    public PG13Scene()
        : base("PG13Scene")
    {
    }

    public override void OnStart()
    {
        Components.Add(Background.Create(0x82));
        Components.Add(Graphic.Create("PG13", 216, 110));
        Components.Add(_timer);
        Components.Add(new Fader()); // color, time, callback function?
        
        _timer.OnStart();
    }

    public override void OnUpdate()
    {
        if (_timer.GetTime() > 300)
        {
            _timer.Stop();
            // TODO: FadeOut

            LoadScene("TitleScene");
        }
        // else if (Inputs.AnyKeyPressed)
        // {
        //     LoadScene("MainMenuScene");
        // }
    }

    // protected override void OnEnd()
    // {
    //     throw new NotImplementedException();
    // }
}