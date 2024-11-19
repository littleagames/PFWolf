using LittleAGames.PFWolf.SDK.Components;

namespace Engine.Managers;

public interface IVideoManager
{
    void Initialize();
    void Update(RenderComponent component);
    void UpdateScreen();

    void LimitFrameRate(int frames);

    void Shutdown();
}