using SDL2;

namespace Engine.Managers;

public class SDLInputManager : IInputManager
{
    public bool IsQuitTriggered { get; private set; } = false;

    public void PollEvents()
    {
        while (SDL.SDL_PollEvent(out var polledEvent) != 0)
        {
            switch (polledEvent.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    IsQuitTriggered = true;
                    break;
                // case SDL.SDL_EventType.SDL_KEYDOWN:
                //     var keyDown = MapKey(polledEvent.key.keysym.sym); //scancode, keymod?
                //     InputHandler.SetKeyDown(keyDown);
                //     break;
                // case SDL.SDL_EventType.SDL_KEYUP:
                //     var keyUp = MapKey(polledEvent.key.keysym.sym); //scancode, keymod?
                //     InputHandler.SetKeyUp(keyUp);
                //     break;
            }
        } 
    }

    public void Initialize(InputComponent component)
    {
    }

    public void Update(InputComponent component)
    {
        // TODO: This isn't exactly registering how I expected
        if (SDL.SDL_PollEvent(out var polledEvent) != 0)
        {
            switch (polledEvent.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    IsQuitTriggered = true;
                    break;
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    var keyDown = MapKey(polledEvent.key.keysym.sym); //scancode, keymod?
                    component.SetKeyDown(keyDown);
                    break;
                case SDL.SDL_EventType.SDL_KEYUP:
                    var keyUp = MapKey(polledEvent.key.keysym.sym); //scancode, keymod?
                    component.SetKeyUp(keyUp);
                    break;
            }
        } 
    }

    private static Keys MapKey(SDL.SDL_Keycode keycode)
    {
        return keycode switch
        {
            SDL.SDL_Keycode.SDLK_UP => Keys.Up,
            SDL.SDL_Keycode.SDLK_DOWN => Keys.Down,
            SDL.SDL_Keycode.SDLK_LEFT => Keys.Left,
            SDL.SDL_Keycode.SDLK_RIGHT => Keys.Right,
            SDL.SDL_Keycode.SDLK_RETURN => Keys.Return,
            SDL.SDL_Keycode.SDLK_ESCAPE => Keys.Escape,
            SDL.SDL_Keycode.SDLK_MINUS => Keys.Minus,
            SDL.SDL_Keycode.SDLK_EQUALS => Keys.Equals,
            SDL.SDL_Keycode.SDLK_a => Keys.A,
            SDL.SDL_Keycode.SDLK_d => Keys.D,
            _ => Keys.None
        };
    }
}