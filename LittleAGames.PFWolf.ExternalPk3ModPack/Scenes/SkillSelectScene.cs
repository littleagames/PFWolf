[PfWolfScene("wolf3d:SkillSelectScene")]
public class SkillSelectScene : MenuScene
{
    private const int MenuX = 50;
    private const int MenuY = 100;
    private const int MenuWidth = 225;
    private const int MenuHeight = 13*4+15;
    private const int MenuIndent = 24;
    private const int LineSpacing = 13;

    private Graphic _skillGraphic;
    
    public SkillSelectScene()
        :base (MenuX, MenuY, MenuWidth, MenuHeight, MenuIndent, LineSpacing)
    {
        Components.Add(Text.Create("How Tough Are You?", MenuX+20, MenuY-32, "LargeFont", 0x47));
        
        Menu.AddMenuItem("Can I Play, Daddy?", ActiveState.Active, SelectSkill);
        Menu.AddMenuItem("Don't Hurt Me.", ActiveState.Active, SelectSkill);
        Menu.AddMenuItem("Bring 'em on!", ActiveState.Active, SelectSkill);
        Menu.AddMenuItem("I am Death incarnate", ActiveState.Active, SelectSkill);
        
        Components.Add(Graphic.Create("c_mouselback", 112, 184));
        Components.Add(Wolf3DBorderedWindow.Create(MenuX-5, MenuY-10, MenuWidth, MenuHeight));
        
        // Added here for priority
        Components.Add(Menu);
        Components.Add(Cursor);
        _skillGraphic = Graphic.Create("c_babymode", MenuX + 185, MenuY + 7);
        Components.Add(_skillGraphic);
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
            UpdateSkillGraphic(Menu.CurrentIndex);
            return;
        }
        
        if (Input.IsKeyDown(Keys.Up))
        {
            Menu.MoveUp();
            Input.ClearKeysDown();
            Cursor.SetPosition(Menu.GetCursorPosition());
            UpdateSkillGraphic(Menu.CurrentIndex);
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
    
    public void SelectSkill()
    {
        var episode = ContextData?.GetProperty<int>("SelectedEpisode") ?? -1;
        LoadScene("wolf3d:GameLoopScene", new SceneContext
        {
            { "SelectedSkill", Menu.CurrentIndex },
            { "SelectedEpisode", episode }
        });
    }

    private void UpdateSkillGraphic(int skillIndex)
    {
        var skillAssets = new[]
        {
            "C_BabyMode",
            "C_Easy",
            "C_Normal",
            "C_Hard"
        };
        
        _skillGraphic.SetAsset(skillAssets[skillIndex]);
    }
}