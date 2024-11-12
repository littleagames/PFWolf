
using LittleAGames.PFWolf.SDK;
using LittleAGames.PFWolf.SDK.Components;

[PfWolfScript("wolf3d:EpisodeSelectScene")]
public class EpisodeSelectScene : MenuScene
{
    private readonly PfTimer _pfTimer = new();
    private readonly Fader _fadeInFader = Fader.Create(1.0f, 0.0f, 0xFF, 0x00, 0x00, 20);
    private readonly Fader _fadeOutFader = Fader.Create(0.0f, 1.0f, 0xFF, 0x00, 0x00, 20);

    public EpisodeSelectScene()
    :base (10, 23, 300, 254, 88, 13)
    {
        Menu.AddMenuItem("Episode 1", ActiveState.Active, NoOp);
        Menu.AddMenuItem("Escape from Wolfenstein", ActiveState.Disabled, null);
        Menu.AddMenuItem("Episode 2", ActiveState.Active, NoOp);
        Menu.AddMenuItem("Operation: Eisenfaust", ActiveState.Disabled, null);
        Menu.AddMenuItem("Episode 3", ActiveState.Active, NoOp);
        Menu.AddMenuItem("Die, Fuhrer, Die!", ActiveState.Disabled, null);
        Menu.AddMenuItem("Episode 4", ActiveState.Active, NoOp);
        Menu.AddMenuItem("A Dark Secret", ActiveState.Disabled, null);
        Menu.AddMenuItem("Episode 5", ActiveState.Active, NoOp);
        Menu.AddMenuItem("Trail of the Madman", ActiveState.Disabled, null);
        Menu.AddMenuItem("Episode 6", ActiveState.Active, NoOp);
        Menu.AddMenuItem("Confrontation", ActiveState.Disabled, null);
        
        Components.Add(Wolf3DBorderedWindow.Create(68, 52, 178, 13*9+6));
        Components.Add(Menu);
        
        Components.Add(Graphic.Create("C_Cursor1", 10, 23+13*0));
        
        Components.Add(_pfTimer);
        Components.Add(_fadeInFader);
        Components.Add(_fadeOutFader);
    }
    
    public void NoOp()
    {
        return;
    }

}