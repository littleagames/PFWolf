using SDL2;
using static SDL2.SDL;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using LittleAGames.PFWolf.SDK.Assets;
using LittleAGames.PFWolf.SDK.Components;

namespace Engine.Managers;

/// <summary>
/// Thread-safe singleton implementation for the Visual Layer manager
/// </summary>
public class VideoLayerManager
{
    private static volatile VideoLayerManager? _instance = null;
    private static object syncRoot = new object();

    public int ScreenWidth { get; private set; } = 960;
    public int ScreenHeight { get; private set; } = 600;
    public int ScreenBits { get; private set; } = 8;
    public bool FullScreen { get; private set; } = false;
    public bool UseDoubleBuffering { get; private set; } = false;
    internal static bool ScreenFaded { get; private set; } = false;

    private VideoLayerManager()
    {
        _ylookup = new uint[ScreenHeight];
    }

    private static int _scaleFactorX;
    private static int _scaleFactorY;

    private SDL_Color[] _currentPalette = new SDL_Color[256];

    private IntPtr _window;
    private IntPtr _renderer;
    private IntPtr _texture;
    private IntPtr _screen;
    private IntPtr _screenBuffer;
    private uint _screenPitch;
    private uint _bufferPitch;

    private uint[] _ylookup = null!;

    private AssetManager _assetManager = null!;
    
    /// <summary>
    /// The instance of the singleton
    /// safe for multithreading
    /// </summary>
    public static VideoLayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // only create a new instance if one doesn't already exist.
                lock (syncRoot)
                {
                    // use this lock to ensure that only one thread can access
                    // this block of code at once.
                    if (_instance == null)
                    {
                        _instance = new VideoLayerManager();
                    }
                }
            }

            return _instance;
        }
    }

    public void Start(AssetManager assetManager)
    {
        _assetManager = assetManager;
        if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_AUDIO | SDL_INIT_JOYSTICK) < 0)
        {
            Console.WriteLine($"There was an issue initializing SDL. {SDL_GetError()}");
        }

        // TODO: Get "Fullscreen" from config
        Initialize("PF Wolf", false);
    }

    public void Initialize(string gameTitle, bool fullscreen = false)
    {
        // Create a new window given a title, size, and passes it a flag indicating it should be shown.
        _window = SDL.SDL_CreateWindow(gameTitle, SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, ScreenWidth, ScreenHeight, (fullscreen ? SDL_WindowFlags.SDL_WINDOW_FULLSCREEN : 0) | SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL);

        if (_window == IntPtr.Zero)
        {
            Console.WriteLine($"There was an issue creating the window. {SDL_GetError()}");
        }

        SDL_PixelFormatEnumToMasks(SDL_PIXELFORMAT_ARGB8888, out var screenBits, out var r, out var g, out var b, out var a);

        _screen = SDL.SDL_CreateRGBSurface(0, ScreenWidth, ScreenHeight, screenBits, r, g, b, a);
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

        SDL_SetRenderDrawBlendMode(_renderer, SDL_BlendMode.SDL_BLENDMODE_BLEND);
        SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, "0");

        SDL_ShowCursor(SDL_DISABLE);
        SDL_Surface sdl_screen = (SDL_Surface)Marshal.PtrToStructure(_screen, typeof(SDL_Surface));
        SDL_PixelFormat sdl_screen_format = (SDL_PixelFormat)Marshal.PtrToStructure(sdl_screen.format, typeof(SDL_PixelFormat));

        SDL_SetPaletteColors(sdl_screen_format.palette, GamePal.BasePalette, 0, 256);

        // Set palette global variable
        Array.Copy(GamePal.BasePalette, _currentPalette, 256);

        _screenBuffer = SDL.SDL_CreateRGBSurface(0, ScreenWidth, ScreenHeight, 8, 0, 0, 0, 0);
        if (_screenBuffer == IntPtr.Zero)
        {
            Console.WriteLine($"There was an issue creating the screenbuffer. {SDL.SDL_GetError()}");
        }
        SDL_Surface sdl_screenbuffer = (SDL_Surface)Marshal.PtrToStructure(_screenBuffer, typeof(SDL_Surface));
        SDL_PixelFormat sdl_screenbuffer_format = (SDL_PixelFormat)Marshal.PtrToStructure(sdl_screenbuffer.format, typeof(SDL_PixelFormat));

        SDL_SetPaletteColors(sdl_screenbuffer_format.palette, GamePal.BasePalette, 0, 256);

        _texture = SDL_CreateTexture(
            _renderer,
            SDL_PIXELFORMAT_ARGB8888,
            (int)SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING,
            ScreenWidth, ScreenHeight);

        _bufferPitch = (uint)sdl_screenbuffer.pitch;
        _scaleFactorX = ScreenWidth / 320;
        _scaleFactorY = ScreenHeight / 200;

        for (var i = 0; i < ScreenHeight; i++)
            _ylookup[i] = (uint)(i * _bufferPitch);
    }

    public void WaitVBL(uint a)
    {
        SDL_Delay((a) * 8);
    }

    public void FadeOut()
    {
        FadeOut(steps: 30);
    }
    public void FadeOut(int steps)
    {
        FadeOut(start: 0, end: 255, red: 0, green: 0, blue: 0, steps);
    }    

    public void FadeIn()
    {
        FadeIn(30);
    }
    public void FadeIn(int steps)
    {
        FadeIn(0, 255, GamePal.BasePalette, steps);
    }

    public void UpdateScreen()
    {
        UpdateScreen(_screenBuffer);
    }
    
    public void DrawBlock(int x, int y, byte[] block)
    {
        MemToScreen(block, 64, 64, x, y);
    }
    //
    // public void DrawTextString(int startX, int startY, string text, FontName fontName, FontColor color)
    // {
    //     var gfxManager = GraphicsManager.Instance;
    //     var font = gfxManager.GetFont(fontName);
    //
    //     var printX = startX;
    //     var printY = startY;
    //
    //     foreach(char textChar in text)
    //     {
    //         var asciiIndex = (int)textChar;
    //         var fontChar = font.Characters[asciiIndex];
    //
    //         if (fontChar.Data.Length > 0)
    //         {
    //             var modifiedFontData = new byte[fontChar.Data.Length];
    //             for (var i = 0; i < fontChar.Data.Length; i++)
    //             {
    //                 var fontFlag = fontChar.Data[i] > 0;
    //                 modifiedFontData[i] = fontFlag ? (byte)color.Value : (byte)0xff;
    //             }
    //
    //             MemToScreen(modifiedFontData, fontChar.Width, fontChar.Height, printX, printY);
    //         }
    //
    //         if (textChar == '\n')
    //         {
    //             printX = startX;
    //             printY = printY + fontChar.Height;
    //             continue;
    //         }
    //
    //         printX += fontChar.Width;
    //     }

        // TODO: Loop through each character in "text"
        // Or I can build the text in byte array size
        //      can send that once to MemToScreen

        //MemToScreen(colorizedFont)

        // get each character and print it to the byte[] pixels

    //}

    // TODO: This should not be public as a byte[], only an "asset", which this handles
    private void MemToScreen(byte[] source, int width, int height, int x, int y)
    {
        MemToScreenScaledCoord(_screenBuffer, source, width, height, _scaleFactorX * x, _scaleFactorY * y);
    }

    private void MemToScreenScaledCoord(IntPtr screenBuffer, byte[] source, int width, int height, int destx, int desty)
    {
        byte[] dest;
        int i, j, sci, scj;
        uint m, n;

        IntPtr dest_ptr = LockSurface(screenBuffer);
        if (dest_ptr == IntPtr.Zero) return;

        int size = ScreenWidth * ScreenHeight; // screen size
        dest = new byte[size];
        Marshal.Copy(dest_ptr, dest, 0, size);

        for (j = 0, scj = 0; j < height; j++, scj += _scaleFactorY)
        {
            for (i = 0, sci = 0; i < width; i++, sci += _scaleFactorX)
            {
                byte col = source[(j * width) + i];
                for (m = 0; m < _scaleFactorY; m++)
                {
                    for (n = 0; n < _scaleFactorX; n++)
                    {
                        //if (col == 0xff) continue;

                        var xlength = sci + n + destx;
                        var ylength = scj + m + desty;
                        if (ylength > _ylookup.Length || (_ylookup[scj + m + desty] + xlength) > dest.Length) return;
                        dest[_ylookup[scj + m + desty] + sci + n + destx] = col;
                    }
                }
            }
        }

        SDL_Surface sdl_screenbuffer = (SDL_Surface)Marshal.PtrToStructure(screenBuffer, typeof(SDL_Surface));
        GCHandle pinnedArray = GCHandle.Alloc(dest, GCHandleType.Pinned);
        IntPtr dest_pointer = pinnedArray.AddrOfPinnedObject();
        sdl_screenbuffer.pixels = dest_pointer;
        Marshal.StructureToPtr(sdl_screenbuffer, screenBuffer, false);

        UnlockSurface(screenBuffer);
    }

    public void Draw(Component component)
    {
        if (component.GetType().IsAssignableTo(typeof(Background))) // TODO: Is this the answer?
        {
            var backgroundRectangle = (Background)component;
            DrawRectangleScaledCoord(0, 0, ScreenWidth, ScreenHeight, backgroundRectangle.Color);
        }
        
        if (component.GetType().IsAssignableTo(typeof(Rectangle))) // TODO: Is this the answer?
        {
            var rect = (Rectangle)component;
            DrawRectangleScaledCoord(rect.X, rect.Y, rect.Width, rect.Height, rect.Color);
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

    private void DrawRectangleScaledCoord(int scaledX, int scaledY, int scaledWidth, int scaledHeight, byte color)
    {
        byte[] dest;
        IntPtr dest_ptr = LockSurface(_screenBuffer);
        if (dest_ptr == IntPtr.Zero) return;

        int size = ScreenWidth * ScreenHeight; // screen size
        dest = new byte[size];
        Marshal.Copy(dest_ptr, dest, 0, size);

        if (scaledY > _ylookup.Length) return;
        var firstPosition = _ylookup[scaledY] + scaledX;
        var position = firstPosition;

        for (int i = 0; i < scaledHeight; i++)
        {
            //memset(dest, color, scwidth);
            for (int scw = 0; scw < scaledWidth; scw++)
            {
                if (position + scw > dest.Length) continue;

                dest[position + scw] = color;
            }

            position += _bufferPitch;
        }

        SDL_Surface sdl_screenbuffer = (SDL_Surface)Marshal.PtrToStructure(_screenBuffer, typeof(SDL_Surface));
        GCHandle pinnedArray = GCHandle.Alloc(dest, GCHandleType.Pinned);
        IntPtr dest_pointer = pinnedArray.AddrOfPinnedObject();
        sdl_screenbuffer.pixels = dest_pointer;
        Marshal.StructureToPtr(sdl_screenbuffer, _screenBuffer, false);
        UnlockSurface(_screenBuffer);
    }

    public void Shutdown() // Dispose?
    {
        // Clean up the resources that were created.
        SDL_FreeSurface(_screen);
        SDL_FreeSurface(_screenBuffer);
        SDL.SDL_DestroyTexture(_texture);
        SDL.SDL_DestroyRenderer(_renderer);
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_Quit();
    }

    #region Private Methods
    /// <summary>
    /// Fills the palette with a single color
    /// </summary>
    /// <param name="red"></param>
    /// <param name="green"></param>
    /// <param name="blue"></param>
    private void FillPalette(byte red, byte green, byte blue)
    {
        int i;
        SDL_Color[] pal = new SDL_Color[256];

        for (i = 0; i < 256; i++)
        {
            pal[i].r = red;
            pal[i].g = green;
            pal[i].b = blue;
        }

        SetPalette(pal, true);
    }

    private void SetPalette(SDL_Color[] palette, bool forceupdate)
    {
        Array.Copy(palette, _currentPalette, 256);

        if (ScreenBits == 8) // This shouldn't be a constant, it should be a gamesetting
        {
            // TODO: Null checking on these
            SDL_Surface sdl_screen = (SDL_Surface)Marshal.PtrToStructure(_screen, typeof(SDL_Surface));
            SDL_PixelFormat sdl_screen_format = (SDL_PixelFormat)Marshal.PtrToStructure(sdl_screen.format, typeof(SDL_PixelFormat));

            SDL_SetPaletteColors(sdl_screen_format.palette, palette, 0, 256);
        }

        SDL_Surface sdl_screenBuffer = (SDL_Surface)Marshal.PtrToStructure(_screenBuffer, typeof(SDL_Surface));
        SDL_PixelFormat sdl_screenBuffer_format = (SDL_PixelFormat)Marshal.PtrToStructure(sdl_screenBuffer.format, typeof(SDL_PixelFormat));

        SDL_SetPaletteColors(sdl_screenBuffer_format.palette, palette, 0, 256);

        if (forceupdate)
        {
            UpdateScreen(_screenBuffer);
        }
    }

    private void UpdateScreen(IntPtr surface)
    {
        SDL_BlitSurface(surface, ref Unsafe.NullRef<SDL_Rect>(), _screen, ref Unsafe.NullRef<SDL_Rect>());

        Present(_screen);
    }

    private void Present(IntPtr screen)
    {
        SDL_Surface sdl_screen = (SDL_Surface)Marshal.PtrToStructure(screen, typeof(SDL_Surface));
        SDL_UpdateTexture(_texture, ref Unsafe.NullRef<SDL_Rect>(), sdl_screen.pixels, ScreenWidth * sizeof(uint));
        SDL_RenderClear(_renderer);
        SDL_RenderCopy(_renderer, _texture, ref Unsafe.NullRef<SDL_Rect>(), ref Unsafe.NullRef<SDL_Rect>());
        SDL_RenderPresent(_renderer);
    }
    private IntPtr LockSurface(IntPtr surface)
    {
        if (SDL_MUSTLOCK(surface))
        {
            if (SDL_LockSurface(surface) < 0)
                return IntPtr.Zero;
        }

        SDL_Surface sdl_surface = (SDL_Surface)Marshal.PtrToStructure(surface, typeof(SDL_Surface));
        return sdl_surface.pixels;
    }

    private void UnlockSurface(IntPtr surface)
    {
        if (SDL_MUSTLOCK(surface))
        {
            SDL_UnlockSurface(surface);
        }
    }
    private byte GetPixel(int x, int y)
    {
        byte col;

        IntPtr dest_ptr = LockSurface(_screenBuffer);
        if (dest_ptr == IntPtr.Zero)
            return 0;

        int size = ScreenWidth * ScreenHeight; // screen size
        var dest = new byte[size];
        Marshal.Copy(dest_ptr, dest, 0, size);
        col = dest[_ylookup[y] + x];

        UnlockSurface(_screenBuffer);

        return col;
    }

    private void FadeOut(int start, int end, byte red, byte green, byte blue, int steps)
    {
        int i, j, orig, delta;
        SDL_Color[] palette1 = new SDL_Color[256];
        SDL_Color[] palette2 = new SDL_Color[256];

        WaitVBL(1); // wait 8 tics

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

            if (!UseDoubleBuffering || ScreenBits == 8) WaitVBL(1);
            SetPalette(palette2, true);
        }

        //
        // final color
        //
        FillPalette(red, green, blue);

        ScreenFaded = true;
    }

    private void FadeIn(int start, int end, SDL_Color[] palette, int steps)
    {
        int i, j, delta;
        SDL_Color[] palette1 = new SDL_Color[256];
        SDL_Color[] palette2 = new SDL_Color[256];

        WaitVBL(1);
        Array.Copy(_currentPalette, palette1, 256);
        Array.Copy(palette1, palette2, 256);

        //
        // fade through intermediate frames
        //
        for (i = 0; i < steps; i++)
        {
            for (j = start; j <= end; j++)
            {
                delta = palette[j].r - palette1[j].r;
                palette2[j].r = (byte)(palette1[j].r + delta * i / steps);
                delta = palette[j].g - palette1[j].g;
                palette2[j].g = (byte)(palette1[j].g + delta * i / steps);
                delta = palette[j].b - palette1[j].b;
                palette2[j].b = (byte)(palette1[j].b + delta * i / steps);
            }

            if (!UseDoubleBuffering || ScreenBits == 8) WaitVBL(1);
            SetPalette(palette2, true);
        }

        //
        // final color
        //
        SetPalette(palette, true);
        ScreenFaded = false;
    }

    #endregion
}
