using LittleAGames.PFWolf.SDK;
using LittleAGames.PFWolf.SDK.Components;

[PfWolfScript("wolf3d:MainMenuScene")]
public class MainMenuScene : Scene
{
    private readonly PfTimer _pfTimer = new();
    private readonly Fader _fadeInFader = Fader.Create(1.0f, 0.0f, 0x00, 0x00, 0x00, 20);
    private readonly Fader _fadeOutFader = Fader.Create(0.0f, 1.0f, 0x00, 0x00, 0x00, 20);
    private readonly Text _newGameText = Text.Create("New Game", 76+24, 55+13*0, "LargeFont", 0x13);
    private readonly Text _soundText = Text.Create("Sound", 76+24, 55+13*1, "LargeFont", 0x17);
    private readonly Text _ctrlText = Text.Create("Control", 76+24, 55+13*2, "LargeFont", 0x17);
    private readonly Text _loadGameText = Text.Create("Load Game", 76+24, 55+13*3, "LargeFont", 0x17);
    private readonly Text _saveGameText = Text.Create("Save Game", 76+24, 55+13*4, "LargeFont", 0x2b);
    private readonly Text _changeViewText = Text.Create("Change View", 76+24, 55+13*5, "LargeFont", 0x17);
    private readonly Text _readThisText = Text.Create("Read This!", 76+24, 55+13*6, "LargeFont", 0x4a);
    private readonly Text _backToGameText = Text.Create("Back to Demo", 76+24, 55+13*7, "LargeFont", 0x17);
    private readonly Text _quitText = Text.Create("Quit", 76+24, 55+13*8, "LargeFont", 0x17);
    
    public MainMenuScene()
    {
    }

    public override void OnStart()
    {
        Components.Add(Background.Create(0x29));
        Components.Add(Graphic.Create("c_mouselback", 112, 184));
        Components.Add(Stripe.Create(10, 0x2c));
        Components.Add(Graphic.Create("c_options", 84, 0));
        Components.Add(Wolf3DBorderedWindow.Create(68, 52, 178, 13*9+6));
        
        Components.Add(Graphic.Create("C_Cursor1", 72, 55+13*0));
        Components.Add(_newGameText);
        Components.Add(_soundText);
        Components.Add(_ctrlText);
        Components.Add(_loadGameText);
        Components.Add(_saveGameText);
        Components.Add(_changeViewText);
        Components.Add(_readThisText);
        Components.Add(_backToGameText);
        Components.Add(_quitText);
        
        Components.Add(_pfTimer);
        Components.Add(_fadeInFader);
        Components.Add(_fadeOutFader);
    }

    public override void OnUpdate()
    {
        if (!_fadeInFader.IsFading)
            _fadeInFader.BeginFade();

        if (!_fadeInFader.IsComplete)
            return;
    }

    // protected override void OnDestroy()
    // {
    //     throw new NotImplementedException();
    // }
}