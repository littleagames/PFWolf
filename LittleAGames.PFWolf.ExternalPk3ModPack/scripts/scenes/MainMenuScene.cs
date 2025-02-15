[PfWolfScene("wolf3d:MainMenuScene")]
public class MainMenuScene : MenuScene
{
    private const int MenuX = 76;
    private const int MenuY = 55;
    private const int MenuWidth = 178;
    private const int MenuHeight = 13 * 9 + 6;
    private const int MenuIndent = 24;
    private const int LineSpacing = 13;
    public MainMenuScene() 
        : base (MenuX, MenuY, MenuWidth, MenuHeight, MenuIndent, LineSpacing)
    {
        Menu.AddMenuItem("New Game", ActiveState.Active, NewGame);
        Menu.AddMenuItem("Sound", ActiveState.Active, NoOp);
        Menu.AddMenuItem("Control", ActiveState.Active, NoOp);
        Menu.AddMenuItem("Load Game", ActiveState.Active, NoOp);
        Menu.AddMenuItem("Save Game", ActiveState.Disabled, NoOp);
        Menu.AddMenuItem("Change View", ActiveState.Active, NoOp);
        Menu.AddMenuItem("Read This!", ActiveState.Special, NoOp);
        Menu.AddMenuItem("Back to Demo", ActiveState.Active, NoOp);
        Menu.AddMenuItem("Quit", ActiveState.Active, NoOp);
        
        Components.Add(Graphic.Create("c_mouselback", 112, 184));
        Components.Add(Stripe.Create(10, 0x2c));
        Components.Add(Graphic.Create("c_options", 84, 0));
        Components.Add(Wolf3DBorderedWindow.Create(MenuX-8, MenuY-3, MenuWidth, MenuHeight));
        
        // Added here for priority
        Components.Add(Menu);
        Components.Add(Cursor);
    }

    public void NewGame()
    {
        // TODO: Fade out
        
        var sceneName = GetSceneName();
        LoadScene("wolf3d:EpisodeSelectScene", new SceneContext
        {
            { "ParentMenu:Scene", sceneName },
            { "ParentMenu:LastIndex", Menu.CurrentIndex }
        });
    }

    public void NoOp()
    {
        return;
    }

    public override void OnStart()
    {
        if (!FadeInFader.IsFading)
            FadeInFader.BeginFade();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (!FadeInFader.IsComplete)
            return;

        // TODO: This should be in a HandleMenu call
        if (Input.IsKeyDown(Keys.Down))
        {
            Menu.MoveDown();
            Input.ClearKeysDown();
            Cursor.SetPosition(Menu.GetCursorPosition());
            return;
        }
        
        if (Input.IsKeyDown(Keys.Up))
        {
            Menu.MoveUp();
            Input.ClearKeysDown();
            Cursor.SetPosition(Menu.GetCursorPosition());
            return;
        }
        
        if (Input.IsKeyDown(Keys.Return))
        {
            Menu.PerformAction(); // GoToMenu(menu.ActionMenu)
            Input.ClearKeysDown();
            Cursor.SetPosition(Menu.GetCursorPosition());
            return;
        }
        
        if (Input.IsKeyDown(Keys.Escape))
        {
            //Menu.PerformExitAction(Menu.ParentMenu); // TODO: Action would either be a "GoToMenu(menu.ParentMenu)" or "ConfirmDialog"
            Input.ClearKeysDown();
            Cursor.SetPosition(Menu.GetCursorPosition());
            return;
        }
    }
}