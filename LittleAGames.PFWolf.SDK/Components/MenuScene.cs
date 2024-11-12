namespace LittleAGames.PFWolf.SDK.Components;

public class MenuScene : Scene
{
    protected Fader FadeInFader { get; }
    protected Fader FadeOutFader { get; }
    protected Graphic Cursor { get; }
    
    protected Menu Menu { get; init; }

    public MenuScene(int x, int y, int width, int height, int menuIndent, int lineSpacing)
    {
        FadeInFader = Fader.Create(1.0f, 0.0f, 0xFF, 0x00, 0x00, 20);
        FadeOutFader = Fader.Create(0.0f, 1.0f, 0xFF, 0x00, 0x00, 20);
        Menu = Menu.Create(x, y, menuIndent, lineSpacing);
        Components.Add(Background.Create(0x29));
        Components.Add(Graphic.Create("c_mouselback", 112, 184));
        Components.Add(Stripe.Create(10, 0x2c));
        Components.Add(Wolf3DBorderedWindow.Create(x-8, y-3, width, height));
        Components.Add(Menu);

        Cursor = Graphic.Create("C_Cursor1", 72, 55 + 13 * 0);
        Components.Add(Cursor);
        
        Components.Add(FadeInFader);
        Components.Add(FadeOutFader);
    }
}