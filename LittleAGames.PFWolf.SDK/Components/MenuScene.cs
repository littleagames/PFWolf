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

        Cursor = Graphic.Create("C_Cursor1", Menu.GetCursorPosition());
        
        Components.Add(FadeInFader);
        Components.Add(FadeOutFader);
    }

    protected void ReturnToParent()
    {
        
        if (ContextData == null)
            return;

        var scene = ContextData.GetProperty<string?>("ParentMenu:Scene");
        var lastIndex = ContextData.GetProperty<int?>("ParentMenu:LastIndex");
        if (!string.IsNullOrWhiteSpace(scene))
        {
            LoadScene(scene, new SceneContext
            {
                { "CurrentIndex", lastIndex ?? 0 }
            });
        }
    }
}