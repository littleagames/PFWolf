[PfWolfScript("wolf3d:MainMenuScene")]
public class MainMenuScene : MenuScene
{
    public MainMenuScene() 
        : base (76, 55, 178, 13*9+6, 24, 13)
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
        
        Components.Add(Graphic.Create("c_options", 84, 0));
    }

    public void NewGame()
    {
        // TODO: Store data in the scene
        /*
         * { ParentMenu: { Scene: "wolf3d:MainMenuScene", MenuIndex: CurrentIndex } }
         * 
         */
        LoadScene("wolf3d:EpisodeSelectScene"/*, data*/);
        return;
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

    public override void OnUpdate()
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
            return;
        }
    }

    // protected override void OnDestroy()
    // {
    //     throw new NotImplementedException();
    // }
}