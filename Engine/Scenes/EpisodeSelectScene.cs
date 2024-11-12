[PfWolfScene("wolf3d:EpisodeSelectScene")]
public class EpisodeSelectScene : MenuScene
{
    private const int MenuX = 10;
    private const int MenuY = 23;
    private const int MenuWidth = 300;
    private const int MenuHeight = 154;
    private const int MenuIndent = 88;
    private const int LineSpacing = 13;
    
    public EpisodeSelectScene()
        :base (MenuX, MenuY, MenuWidth, MenuHeight, MenuIndent, LineSpacing)
    {
        Menu.AddMenuItem("Episode 1\nEscape from Wolfenstein", ActiveState.Active, SelectEpisode);
        Menu.AddMenuItem("", ActiveState.Disabled, null);
        Menu.AddMenuItem("Episode 2\nOperation: Eisenfaust", ActiveState.Active, SelectEpisode);
        Menu.AddMenuItem("", ActiveState.Disabled, null);
        Menu.AddMenuItem("Episode 3\nDie, Fuhrer, Die!", ActiveState.Active, SelectEpisode);
        Menu.AddMenuItem("", ActiveState.Disabled, null);
        Menu.AddMenuItem("Episode 4\nA Dark Secret", ActiveState.Active, SelectEpisode);
        Menu.AddMenuItem("", ActiveState.Disabled, null);
        Menu.AddMenuItem("Episode 5\nTrail of the Madman", ActiveState.Active, SelectEpisode);
        Menu.AddMenuItem("", ActiveState.Disabled, null);
        Menu.AddMenuItem("Episode 6\nConfrontation", ActiveState.Active, SelectEpisode);
        Menu.AddMenuItem("", ActiveState.Disabled, null);
        
        Components.Add(Graphic.Create("c_mouselback", 112, 184));
        Components.Add(Wolf3DBorderedWindow.Create(MenuX-4, MenuY-4, MenuWidth+8, MenuHeight+8));
        
        // Added here for priority
        Components.Add(Menu);
        Components.Add(Cursor);
        
        // TODO: Orientation.Centered
        Components.Add(Text.Create("Which Episode to Play?", MenuX, 2, "LargeFont", 0x47));
        Components.Add(Graphic.Create("c_episode1", MenuX + 32, MenuY));
        Components.Add(Graphic.Create("c_episode2", MenuX + 32, MenuY + 1 * 26));
        Components.Add(Graphic.Create("c_episode3", MenuX + 32, MenuY + 2 * 26));
        Components.Add(Graphic.Create("c_episode4", MenuX + 32, MenuY + 3 * 26));
        Components.Add(Graphic.Create("c_episode5", MenuX + 32, MenuY + 4 * 26));
        Components.Add(Graphic.Create("c_episode6", MenuX + 32, MenuY + 5 * 26));
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
            ReturnToParent();
            return;
        }
    }
    
    public void SelectEpisode()
    {
        // TODO: Check ContextData["IsInGame"]
        var sceneName = GetSceneName();
        LoadScene("wolf3d:SkillSelectScene", new SceneContext
        {
            { "ParentMenu:Scene", sceneName },
            { "ParentMenu:LastIndex", Menu.CurrentIndex },
            { "SelectedEpisode", Menu.CurrentIndex / 2 }
        });
    }
}