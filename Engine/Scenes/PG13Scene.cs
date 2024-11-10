using LittleAGames.PFWolf.SDK;
using LittleAGames.PFWolf.SDK.Components;
using Timer = LittleAGames.PFWolf.SDK.Components.Timer;

[PfWolfScript("wolf3d:PG13Scene")]
public class PG13Scene : TitleCardScene
{
    public PG13Scene()
        : base("wolf3d:TitleScene", "wolf3d:MainMenuScene", true, true, 300)
    {
    }

    public override void OnStart()
    {
        Components.Add(Background.Create(0x82));
        Components.Add(Graphic.Create("PG13", 216, 110));
        base.OnStart();
    }
}