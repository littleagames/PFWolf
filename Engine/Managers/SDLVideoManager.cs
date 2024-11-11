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

    private readonly uint[] _yLookup;

    // SDL pointers
    private IntPtr _screen, _screenBuffer, _texture, _renderer, _window;

    public SDLVideoManager(AssetManager assetManager, GameConfiguration config)
    {
        _assetManager = assetManager;
        _config = config;
        
        _gamePalette = LoadPaletteFromAssets(_config.GamePalette);
        _currentPalette = new SDL_Color[_gamePalette.Length];
        Array.Copy(_gamePalette, _currentPalette, 256);
        
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
        SDL_Surface sdlScreen = (SDL.SDL_Surface)Marshal.PtrToStructure(_screen, typeof(SDL.SDL_Surface))!;
        SDL_PixelFormat sdlScreenFormat = (SDL.SDL_PixelFormat)Marshal.PtrToStructure(sdlScreen.format, typeof(SDL.SDL_PixelFormat))!;
       
        SDL_SetPaletteColors(sdlScreenFormat.palette, _currentPalette, 0, 256);

        // Set palette global variable

        _screenBuffer = SDL_CreateRGBSurface(0, _screenSize.Width, _screenSize.Height, 8, 0, 0, 0, 0);
        if (_screenBuffer == IntPtr.Zero)
        {
            Console.WriteLine($"There was an issue creating the screenbuffer. {SDL.SDL_GetError()}");
        }
        SDL_Surface sdlScreenBuffer = (SDL.SDL_Surface)Marshal.PtrToStructure(_screenBuffer, typeof(SDL.SDL_Surface))!;
        SDL_PixelFormat sdlScreenBufferFormat = (SDL.SDL_PixelFormat)Marshal.PtrToStructure(sdlScreenBuffer.format, typeof(SDL.SDL_PixelFormat))!;

        SDL_SetPaletteColors(sdlScreenBufferFormat.palette, _currentPalette, 0, 256);

        _texture = SDL_CreateTexture(
            _renderer,
            SDL_PIXELFORMAT_ARGB8888,
            (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING,
            _screenSize.Width, _screenSize.Height);

        _bufferPitch = (uint)sdlScreenBuffer.pitch;
        
        for (var i = 0; i < _screenSize.Height; i++)
            _yLookup[i] = (uint)(i * _bufferPitch);
        
        _isInitialized = true;
    }

    public void Update(Component component)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("Video Manager is not initialized");

        if (component.GetType().IsAssignableTo(typeof(Rectangle)))
        {
            var rect = (Rectangle)component;
            DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, rect.Color);
            return;
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
            return;
        }

        if (component.GetType().IsAssignableTo(typeof(Text)))
        {
            var text = (Text)component;
            var fontAsset = _assetManager.FindAsset(AssetType.Font, text.FontAssetName) as FontAsset;
            if (fontAsset == null)
            {
                // TODO: Placeholder for missing font?
                return;
            }
            
            DrawTextString(text.X, text.Y, text.String, fontAsset, text.Color);
        }

        if (component.GetType().IsAssignableTo(typeof(Fader)))
        {
            var fader = (Fader)component;
            if (fader.IsFading)
            {
                var shiftedPalette = ShiftPalette(fader.Red, fader.Green, fader.Blue, fader.CurrentOpacity);
                SetPalette(shiftedPalette, true);
            }
            
            return;
        }
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

    private SDL_Color[] ShiftPalette(byte red, byte green, byte blue, float opacity)
    {
        SDL_Color[] palette1 = new SDL_Color[256];

        Array.Copy(_gamePalette, palette1, 256);
        // start and end are 0 to 255
        // its possible to only fade out specific colors of the palette
        for (var i = 0; i <= 255; i++)
        {
            var originalColor = _gamePalette[i];
            palette1[i].r = NormalizeColorByte(originalColor.r, red, opacity);
            palette1[i].g = NormalizeColorByte(originalColor.g, green, opacity);
            palette1[i].b = NormalizeColorByte(originalColor.b, blue, opacity);
        }

        return palette1;
    }

    private static byte NormalizeColorByte(byte originalColor, byte destinationColor, float opacity)
    {
        var delta = destinationColor - originalColor;
        return (byte)((originalColor + delta * opacity));
    }
    
    private void DrawTextString(int startX, int startY, string text, FontAsset font, byte color)
    {
        var printX = startX;
        var printY = startY;
    
        foreach(char textChar in text)
        {
            var asciiIndex = (int)textChar;
            var fontChar = font.FontCharacters[asciiIndex];
    
            if (fontChar.RawData.Length > 0)
            {
                var modifiedFontData = new byte[fontChar.RawData.Length];
                for (var i = 0; i < fontChar.RawData.Length; i++)
                {
                    var fontFlag = fontChar.RawData[i] > 0;
                    modifiedFontData[i] = fontFlag ? color : (byte)0xff;
                }
    
                MemToScreen(modifiedFontData, fontChar.Width, fontChar.Height, printX, printY);
            }
    
            if (textChar == '\n')
            {
                printX = startX;
                printY = printY + fontChar.Height;
                continue;
            }
    
            printX += fontChar.Width;
        }
    
        // TODO: Loop through each character in "text"
        // Or I can build the text in byte array size
        //      can send that once to MemToScreen
    
        //MemToScreen(colorizedFont)
    
        // get each character and print it to the byte[] pixels
    
    }
    
    private void MemToScreen(byte[] source, int width, int height, int x, int y)
    {
         MemToScreenScaledCoord(source, width, height, ScaleFactorX * x, ScaleFactorY * y);
    }

    private void MemToScreenScaledCoord(byte[] source, int width, int height, int destx, int desty)
    {
        int i, j, sci, scj;
        uint m, n;

        var surfacePtr = LockSurface(_screenBuffer);
        unsafe
        {
            byte* pixels = (byte*)surfacePtr;

            // Set each pixel to a red color (ARGB format)
            
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
                            if (ylength > _yLookup.Length || (_yLookup[scj + m + desty] + xlength) > (_screenSize.Width * _screenSize.Height)) return;
                            pixels[_yLookup[scj + m + desty] + sci + n + destx] = col;
                        }
                    }
                }
            }
        }
        
        UnlockSurface(_screenBuffer);
    }
    
    private void DrawRectangle(int x, int y, int width, int height, byte color)
    {
        DrawRectangleScaledCoord(ScaleFactorX * x, ScaleFactorY * y, ScaleFactorX * width, ScaleFactorY * height, color);
    }
    
    private void DrawRectangleScaledCoord(int scaledX, int scaledY, int scaledWidth, int scaledHeight, byte color)
    {
        var surfacePtr = LockSurface(_screenBuffer);
        unsafe
        {
            int width = _screenSize.Width;
            int height = _screenSize.Height;
            byte* pixels = (byte*)surfacePtr;

            // Set each pixel to a red color (ARGB format)
            for (int y = scaledY; y < height && y < (scaledY+scaledHeight); y++)
            {
                for (int x = scaledX; x < width && x < (scaledX+scaledWidth); x++)
                {
                    pixels[y * width + x] = color;
                }
            }
        }
        
        UnlockSurface(_screenBuffer);
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