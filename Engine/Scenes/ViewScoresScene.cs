using LittleAGames.PFWolf.SDK;
using LittleAGames.PFWolf.SDK.Components;

[PfWolfScript("wolf3d:ViewScoresScene")]
public class ViewScoresScene : TitleCardScene
{
    public ViewScoresScene()
        : base("wolf3d:MainMenuScene", "wolf3d:MainMenuScene", true, true, 300)
    {
    }

    public override void OnStart()
    {
        Components.Add(Background.Create(0x29));
        Components.Add(Stripe.Create(10, 0x2c));
        Components.Add(Graphic.Create("highscores", 48, 0));
        Components.Add(Graphic.Create("c_name", 24, 68));
        Components.Add(Graphic.Create("c_level", 160, 68));
        Components.Add(Graphic.Create("c_score", 28*8, 68));
        base.OnStart();
    }

    public override void OnUpdate()
    {
    }
}