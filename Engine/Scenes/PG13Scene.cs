using LittleAGames.PFWolf.SDK.Components;
using Timer = LittleAGames.PFWolf.SDK.Components.Timer;

namespace Engine.Scenes;

public class PG13Scene : Scene
{
    private Timer _timer = new();
    
    public PG13Scene()
        : base("PG13Scene")
    {
    }

    public override void OnStart()
    {
        Components.Add(new Background(0x82));
        Components.Add(Graphic.Create("PG13", 216, 110));
        //Components.Add(_timer);
        //_timer.OnStart();
    }

    public override void OnUpdate()
    {
         //if (_timer.GetTime() > 1000 * 3)
         //{
         //    _timer.Stop();
         //    // TODO: FadeOut
             //LoadScene("PG13");
       //  }
        // else if (Inputs.AnyKeyPressed)
        // {
        //     LoadScene("MainMenu");
        // }
    }

    // protected override void OnEnd()
    // {
    //     throw new NotImplementedException();
    // }
}