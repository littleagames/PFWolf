using LittleAGames.PFWolf.SDK;
using LittleAGames.PFWolf.SDK.Components;
using Timer = LittleAGames.PFWolf.SDK.Components.Timer;

[PfWolfScript("wolf3d:TitleScene")]
public class TitleScene : TitleCardScene
{
    public TitleScene()
        : base("wolf3d:CreditsScene", "wolf3d:MainMenuScene", true, true, 300)
    {
    }

    public override void OnStart()
    {
        Components.Add(Graphic.Create("title", 0, 0));
        base.OnStart();
    }
}