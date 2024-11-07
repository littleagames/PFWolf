using LittleAGames.PFWolf.SDK.Components;

namespace Engine.Managers;

public interface IVideoManager
{
    void Initialize();
    void DrawComponent(Component component);
    void UpdateScreen();

    void Shutdown();
}