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
            SDL.SDL_Keycode.SDLK_b => Keys.B,
            SDL.SDL_Keycode.SDLK_c => Keys.C,
            SDL.SDL_Keycode.SDLK_d => Keys.D,
            SDL.SDL_Keycode.SDLK_e => Keys.E,
            SDL.SDL_Keycode.SDLK_f => Keys.F,
            SDL.SDL_Keycode.SDLK_g => Keys.G,
            SDL.SDL_Keycode.SDLK_h => Keys.H,
            SDL.SDL_Keycode.SDLK_i => Keys.I,
            SDL.SDL_Keycode.SDLK_j => Keys.J,
            SDL.SDL_Keycode.SDLK_k => Keys.K,
            SDL.SDL_Keycode.SDLK_l => Keys.L,
            SDL.SDL_Keycode.SDLK_m => Keys.M,
            SDL.SDL_Keycode.SDLK_n => Keys.N,
            SDL.SDL_Keycode.SDLK_o => Keys.O,
            SDL.SDL_Keycode.SDLK_p => Keys.P,
            SDL.SDL_Keycode.SDLK_q => Keys.Q,
            SDL.SDL_Keycode.SDLK_r => Keys.R,
            SDL.SDL_Keycode.SDLK_s => Keys.S,
            SDL.SDL_Keycode.SDLK_t => Keys.T,
            SDL.SDL_Keycode.SDLK_u => Keys.U,
            SDL.SDL_Keycode.SDLK_v => Keys.V,
            SDL.SDL_Keycode.SDLK_w => Keys.W,
            SDL.SDL_Keycode.SDLK_x => Keys.X,
            SDL.SDL_Keycode.SDLK_y => Keys.Y,
            SDL.SDL_Keycode.SDLK_z => Keys.Z,
            SDL.SDL_Keycode.SDLK_LSHIFT => Keys.LeftShift,
            SDL.SDL_Keycode.SDLK_RSHIFT => Keys.RightShift,
            SDL.SDL_Keycode.SDLK_TAB => Keys.Tab,
            SDL.SDL_Keycode.SDLK_BACKQUOTE => Keys.Tilde,
            SDL.SDL_Keycode.SDLK_SPACE => Keys.Space,
            SDL.SDL_Keycode.SDLK_BACKSPACE => Keys.Backspace,
            _ => Keys.None
        };
    }
}