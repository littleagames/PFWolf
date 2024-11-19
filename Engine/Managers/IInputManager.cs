namespace Engine.Managers;

public interface IInputManager
{
    bool IsQuitTriggered { get; }
    void PollEvents();

    void Initialize(InputComponent component);

    void Update(InputComponent component);

}