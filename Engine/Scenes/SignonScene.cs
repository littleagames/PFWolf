using LittleAGames.PFWolf.SDK.Components;
using Timer = LittleAGames.PFWolf.SDK.Components.Timer;

namespace Engine.Scenes;

public class SignonScene : Scene
{
    private Timer _timer = new();
    
    public SignonScene()
        : base("SignonScene")
    {
    }

    public override void OnStart()
    {
        Components.Add(new Graphic("wolf3d-signon", 0, 0));
        //Components.Add(_timer);
        //_timer.OnStart();
    }

    public override void OnUpdate()
    {
        // if (_timer.GetTime() > 1000 * 3)
        // {
        //     _timer.Stop();
        //     //LoadScene("PG13");
        // }
        //else if (Inputs.AnyKeyPressed)
        //{
        //    LoadScene("MainMenu");
        //}
    }

    // protected override void OnEnd()
    // {
    //     throw new NotImplementedException();
    // }
}