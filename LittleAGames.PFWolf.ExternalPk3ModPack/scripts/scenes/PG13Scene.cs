[PfWolfScene("wolf3d:PG13Scene")]
public class PG13Scene : TitleCardScene
{
    public PG13Scene()
        : base("wolf3d:TitleScene", null, true, true, 300)
    {
        Components.Add(Background.Create(0x82));
        Components.Add(Graphic.Create("PG13", 216, 110));
    }
}