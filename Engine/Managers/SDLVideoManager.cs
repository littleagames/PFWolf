using System.Runtime.InteropServices;
using Engine.Extensions;
using LittleAGames.PFWolf.SDK.Assets;
using LittleAGames.PFWolf.SDK.Components;

using SDL2;
using static SDL2.SDL;

namespace Engine.Managers;

public class SDLVideoManager : IVideoManager
{
    private readonly AssetManager _assetManager;
    private readonly GameConfiguration _config;
    private bool _isInitialized = false;
    private readonly SDL_Color[] _gamePalette, _currentPalette;
    private readonly Dimension _screenSize;
    private readonly byte _screenBits;
    private uint _bufferPitch;
    
    // TODO: Eventually scaling will live on the component
    private int ScaleFactorX => _screenSize.Width / 320;
    private int ScaleFactorY => _screenSize.Height / 200;

    private readonly uint[] _yLookup = null!;

    // SDL pointers
    private IntPtr _screen, _screenBuffer, _texture, _renderer, _window;

    public SDLVideoManager(AssetManager assetManager, GameConfiguration config)
    {
        _assetManager = assetManager;
        _config = config;
        _gamePalette = LoadPaletteFromAssets(_config.GamePalette);
        _currentPalette = new SDL_Color[_gamePalette.Length];
        _screenSize = _config.ScreenSize;
        _screenBits = _config.ScreenBits;
        _yLookup = new uint[_screenSize.Height];
         for (var i = 0; i < _screenSize.Height; i++)
             _yLookup[i] = (uint)(i * _bufferPitch);
    }

    public void Initialize()
    {
        if (_isInitialized)
            throw new InvalidOperationException("Video Manager is already initialized");
        
        if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_AUDIO | SDL_INIT_JOYSTICK) < 0)
        {
            throw new Exception($"There was an issue initializing SDL. {SDL_GetError()}");
        }
        
        // Create a new window given a title, size, and passes it a flag indicating it should be shown.
        _window = SDL.SDL_CreateWindow("Wolfenstein 3-D", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, _screenSize.Width, _screenSize.Height, (_config.FullScreen ? SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN : 0) | SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL);

        if (_window == IntPtr.Zero)
        {
            Console.WriteLine($"There was an issue creating the window. {SDL_GetError()}");
        }

        SDL_PixelFormatEnumToMasks(SDL_PIXELFORMAT_ARGB8888, out var screenBits, out var r, out var g, out var b, out var a);

        _screen = SDL.SDL_CreateRGBSurface(0, _screenSize.Width, _screenSize.Height, screenBits, r, g, b, a);
        if (_screen == IntPtr.Zero)
        {
            Console.WriteLine($"There was an issue creating the screen. {SDL_GetError()}");
        }

        // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
        _renderer = SDL.SDL_CreateRenderer(_window,
                                                -1,
                                                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
                                                SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

        if (_renderer == IntPtr.Zero)
        {
            Console.WriteLine($"There was an issue creating the renderer. {SDL.SDL_GetError()}");
        }

        SDL_SetRenderDrawBlendMode(_renderer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
        SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, "0");

        SDL_ShowCursor(SDL_DISABLE);
        SDL.SDL_Surface sdl_screen = (SDL.SDL_Surface)Marshal.PtrToStructure(_screen, typeof(SDL.SDL_Surface));
        SDL.SDL_PixelFormat sdl_screen_format = (SDL.SDL_PixelFormat)Marshal.PtrToStructure(sdl_screen.format, typeof(SDL.SDL_PixelFormat));

        SDL_SetPaletteColors(sdl_screen_format.palette, GamePal.BasePalette, 0, 256);

        // Set palette global variable
        Array.Copy(GamePal.BasePalette, _currentPalette, 256);

        _screenBuffer = SDL.SDL_CreateRGBSurface(0, _screenSize.Width, _screenSize.Height, 8, 0, 0, 0, 0);
        if (_screenBuffer == IntPtr.Zero)
        {
            Console.WriteLine($"There was an issue creating the screenbuffer. {SDL.SDL_GetError()}");
        }
        SDL.SDL_Surface sdl_screenbuffer = (SDL.SDL_Surface)Marshal.PtrToStructure(_screenBuffer, typeof(SDL.SDL_Surface));
        SDL.SDL_PixelFormat sdl_screenbuffer_format = (SDL.SDL_PixelFormat)Marshal.PtrToStructure(sdl_screenbuffer.format, typeof(SDL.SDL_PixelFormat));

        SDL_SetPaletteColors(sdl_screenbuffer_format.palette, GamePal.BasePalette, 0, 256);

        _texture = SDL_CreateTexture(
            _renderer,
            SDL_PIXELFORMAT_ARGB8888,
            (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING,
            _screenSize.Width, _screenSize.Height);

        _bufferPitch = (uint)sdl_screenbuffer.pitch;
        
        for (var i = 0; i < _screenSize.Height; i++)
            _yLookup[i] = (uint)(i * _bufferPitch);
        
        _isInitialized = true;
    }

    public void DrawComponent(Component component)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("Video Manager is not initialized");

        if (component.GetType().IsAssignableTo(typeof(Rectangle)))
        {
            var rect = (Rectangle)component;
            DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, rect.Color);
        }
        
        if (component.GetType().IsAssignableTo(typeof(Graphic)))
        {
            var graphic = (Graphic)component;
            var graphicAsset = _assetManager.FindAsset(AssetType.Graphic, graphic.AssetName) as GraphicAsset;
            if (graphicAsset == null)
            {
                // TODO: Placeholder for missing graphic?
                return;
            }
            MemToScreen(graphicAsset.RawData, graphicAsset.Dimensions.Width, graphicAsset.Dimensions.Height, graphic.X, graphic.Y);
        }
    }

    public void FadeOut()
    {
        FadeOut(steps: 30);
    }
    
    public void FadeOut(int steps)
    {
        FadeOut(start: 0, end: 255, red: 0, green: 0, blue: 0, steps);
    }

    public void UpdateScreen()
    {
        if (!_isInitialized)
            throw new InvalidOperationException("Video Manager is not initialized");
        
        if (_screenBuffer == IntPtr.Zero)
        {
            throw new InvalidOperationException($"There is no screen buffer surface available. {SDL.SDL_GetError()}");
        }
        
        UpdateScreen(_screenBuffer);
    }

    public void Shutdown()
    {
        // Clean up the resources that were created.
        SDL_FreeSurface(_screen);
        SDL_FreeSurface(_screenBuffer);
        SDL_DestroyTexture(_texture);
        SDL_DestroyRenderer(_renderer);
        SDL_DestroyWindow(_window);
        SDL_Quit();
    }
    
    #region Private Methods
    
    private void FadeOut(int start, int end, byte red, byte green, byte blue, int steps)
    {
        int i, j, orig, delta;
        SDL.SDL_Color[] palette1 = new SDL.SDL_Color[256];
        SDL.SDL_Color[] palette2 = new SDL.SDL_Color[256];

        SDL_Delay(1 * 8);
        //WaitVBL(1); // wait 8 tics

        Array.Copy(_currentPalette, palette1, 256);
        Array.Copy(palette1, palette2, 256);

        //
        // fade through intermediate frames
        //
        for (i = 0; i < steps; i++)
        {
            // start and end are 0 to 255
            // its possible to only fade out specific colors of the palette
            for (j = start; j <= end; j++)
            {
                var originalColor = palette1[j];
                orig = originalColor.r;
                delta = red - orig;
                palette2[j].r = (byte)(orig + delta * i / steps);
                orig = originalColor.g;
                delta = green - orig;
                palette2[j].g = (byte)(orig + delta * i / steps);
                orig = originalColor.b;
                delta = blue - orig;
                palette2[j].b = (byte)(orig + delta * i / steps);
            }

            //if (!UseDoubleBuffering || ScreenBits == 8) WaitVBL(1);
            if (_screenBits == 8)
                SDL_Delay(8);
            SetPalette(palette2, true);
        }

        //
        // final color
        //
        FillPalette(red, green, blue);

        //ScreenFaded = true;
    }
    
    /// <summary>
    /// Fills the palette with a single color
    /// </summary>
    /// <param name="red"></param>
    /// <param name="green"></param>
    /// <param name="blue"></param>
    private void FillPalette(byte red, byte green, byte blue)
    {
        int i;
        SDL_Color[] pal = new SDL.SDL_Color[256];

        for (i = 0; i < 256; i++)
        {
            pal[i].r = red;
            pal[i].g = green;
            pal[i].b = blue;
        }

        SetPalette(pal, true);
    }

    private void MemToScreen(byte[] source, int width, int height, int x, int y)
    {
         MemToScreenScaledCoord(_screenBuffer, source, width, height, ScaleFactorX * x, ScaleFactorY * y);
    }

    private void MemToScreenScaledCoord(IntPtr screenBuffer, byte[] source, int width, int height, int destx, int desty)
    {
        byte[] dest;
        int i, j, sci, scj;
        uint m, n;

        IntPtr dest_ptr = LockSurface(screenBuffer);
        if (dest_ptr == IntPtr.Zero) return;

        int size = _screenSize.Width * _screenSize.Height; // screen size
        dest = new byte[size];
        Marshal.Copy(dest_ptr, dest, 0, size);

        for (j = 0, scj = 0; j < height; j++, scj += ScaleFactorX)
        {
            for (i = 0, sci = 0; i < width; i++, sci += ScaleFactorX)
            {
                byte col = source[(j * width) + i];
                for (m = 0; m < ScaleFactorY; m++)
                {
                    for (n = 0; n < ScaleFactorX; n++)
                    {
                        if (col == 0xff) continue;

                        var xlength = sci + n + destx;
                        var ylength = scj + m + desty;
                        if (ylength > _yLookup.Length || (_yLookup[scj + m + desty] + xlength) > dest.Length) return;
                        dest[_yLookup[scj + m + desty] + sci + n + destx] = col;
                    }
                }
            }
        }

        SDL_Surface sdlScreenBuffer = (SDL_Surface)Marshal.PtrToStructure(screenBuffer, typeof(SDL_Surface))!;
        
        IntPtr ptr = Marshal.AllocHGlobal(dest.Length);
        Marshal.Copy(dest, 0, ptr, dest.Length);
        sdlScreenBuffer.pixels = ptr;
        
        Marshal.StructureToPtr(sdlScreenBuffer, screenBuffer, false);
        
        UnlockSurface(screenBuffer);
        // Free the allocated unmanaged memory
        Marshal.FreeHGlobal(ptr);
    }
    
    private void DrawRectangle(int x, int y, int width, int height, byte color)
    {
        DrawRectangleScaledCoord(ScaleFactorX * x, ScaleFactorY * y, ScaleFactorX * width, ScaleFactorY * height, color);
    }
    
    private void DrawRectangleScaledCoord(int scaledX, int scaledY, int scaledWidth, int scaledHeight, byte color)
    {
        byte[] dest;
        IntPtr dest_ptr = LockSurface(_screenBuffer);
        if (dest_ptr == IntPtr.Zero) return;

        int size = _screenSize.Width * _screenSize.Height; // screen size
        dest = new byte[size];
        Marshal.Copy(dest_ptr, dest, 0, size);

        if (scaledY > _yLookup.Length) return;
        var firstPosition = _yLookup[scaledY] + scaledX;
        var position = firstPosition;

        for (int i = 0; i < scaledHeight; i++)
        {
            for (int scw = 0; scw < scaledWidth; scw++)
            {
                if ((position + scw) >= dest.Length)
                    continue;

                dest[position + scw] = color;
            }

            position += _bufferPitch;
        }

        SDL_Surface sdlScreenBuffer = (SDL_Surface)Marshal.PtrToStructure(_screenBuffer, typeof(SDL_Surface))!;
        
        IntPtr ptr = Marshal.AllocHGlobal(dest.Length);
        Marshal.Copy(dest, 0, ptr, dest.Length);
        sdlScreenBuffer.pixels = ptr;
        
        Marshal.StructureToPtr(sdlScreenBuffer, _screenBuffer, false);
        
        UnlockSurface(_screenBuffer);
        // Free the allocated unmanaged memory
        Marshal.FreeHGlobal(ptr);
    }
    
    private IntPtr LockSurface(IntPtr surface)
    {
        if (SDL_MUSTLOCK(surface))
        {
            if (SDL_LockSurface(surface) < 0)
                return IntPtr.Zero;
        }

        SDL_Surface sdlSurface = (SDL_Surface)Marshal.PtrToStructure(surface, typeof(SDL_Surface))!;
        return sdlSurface.pixels;
    }

    private void UnlockSurface(IntPtr surface)
    {
        if (SDL_MUSTLOCK(surface))
        {
            SDL_UnlockSurface(surface);
        }
    }
     
    private SDL_Color[] LoadPaletteFromAssets(string paletteAssetName)
    {
        var palette = _assetManager.FindAsset(AssetType.Palette, paletteAssetName) as PaletteAsset;
        if (palette == null)
            throw new InvalidDataException($"Palette {paletteAssetName} not found. Did the pfwolf.pk3 load correctly?");

        return palette.ToSDLPalette();
    }
    
    private void SetPalette(SDL_Color[] palette, bool forceupdate)
    {
        Array.Copy(palette, _currentPalette, 256);

        if (_screenBits == 8)
        {
            SDL_Surface sdlScreen = (SDL_Surface)Marshal.PtrToStructure(_screen, typeof(SDL_Surface))!;
            SDL_PixelFormat sdlScreenFormat = (SDL_PixelFormat)Marshal.PtrToStructure(sdlScreen.format, typeof(SDL_PixelFormat))!;
            SDL_SetPaletteColors(sdlScreenFormat.palette, palette, 0, 256);
        }

        SDL_Surface sdlScreenBuffer = (SDL_Surface)Marshal.PtrToStructure(_screenBuffer, typeof(SDL_Surface))!;
        SDL_PixelFormat sdlScreenBufferFormat = (SDL_PixelFormat)Marshal.PtrToStructure(sdlScreenBuffer.format, typeof(SDL_PixelFormat))!;
        SDL_SetPaletteColors(sdlScreenBufferFormat.palette, palette, 0, 256);

        if (forceupdate)
        {
            UpdateScreen(_screenBuffer);
        }
    }

    private void UpdateScreen(IntPtr surface)
    {
        // Blit the image surface to the window surface
        SDL_BlitSurface(surface, IntPtr.Zero, _screen, IntPtr.Zero);

        Present(_screen);
    }
    private void Present(IntPtr screen)
    {
        SDL_Surface sdlScreen = (SDL_Surface)Marshal.PtrToStructure(screen, typeof(SDL_Surface))!;
        SDL_UpdateTexture(_texture, IntPtr.Zero, sdlScreen.pixels, _screenSize.Width * sizeof(uint));
        SDL_RenderClear(_renderer);
        SDL_RenderCopy(_renderer, _texture, IntPtr.Zero, IntPtr.Zero);
        SDL_RenderPresent(_renderer);
    }
    #endregion
}