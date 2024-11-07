﻿using LittleAGames.PFWolf.SDK.Components;
using Timer = LittleAGames.PFWolf.SDK.Components.Timer;

namespace Engine.Scenes;

public class SignonScene : Scene
{
    private readonly Timer _timer = new();
    
    public SignonScene()
        : base("SignonScene")
    {
    }

    public override void OnStart()
    {
        Components.Add(Graphic.Create("wolf3d-signon", 0, 0));
        Components.Add(_timer);
        Components.Add(new Fader()); // color, time, callback function?
        
        _timer.OnStart();
    }

    public override void OnUpdate()
    {
         if (_timer.GetTime() > 300 /*|| Inputs.AnyKeyPressed*/)
         {
             _timer.Stop();
             // TODO: FadeOut

             LoadScene("Pg13Scene");
         }
    }

    // protected override void OnEnd()
    // {
    //     throw new NotImplementedException();
    // }
}