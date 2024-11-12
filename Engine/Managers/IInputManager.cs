using LittleAGames.PFWolf.SDK.Handlers;

namespace Engine.Managers;

public interface IInputManager
{
    InputHandler InputHandler { get; }
    bool IsQuitTriggered { get; }

    void PollEvents();
}