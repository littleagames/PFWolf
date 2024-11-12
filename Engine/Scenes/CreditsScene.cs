[PfWolfScene("wolf3d:CreditsScene")]
public class CreditsScene : TitleCardScene
{
    public CreditsScene()
        : base("wolf3d:ViewScoresScene", "wolf3d:MainMenuScene", true, true, 300)
    {
        Components.Add(Graphic.Create("credits", 0, 0));
    }
}