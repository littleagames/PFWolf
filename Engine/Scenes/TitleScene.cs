[PfWolfScene("wolf3d:TitleScene")]
public class TitleScene : TitleCardScene
{
    public TitleScene()
        : base("wolf3d:CreditsScene", "wolf3d:MainMenuScene", true, true, 300)
    {
        Components.Add(Graphic.Create("title", 0, 0));
    }
}