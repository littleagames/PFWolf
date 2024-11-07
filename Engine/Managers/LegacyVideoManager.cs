using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using LittleAGames.PFWolf.SDK.Assets;
using LittleAGames.PFWolf.SDK.Components;
using SDL2;
using static SDL2.SDL;

namespace Engine.Managers;

public class LegacyVideoManager
{
    public int ScreenWidth { get; private set; } = 960;
    public int ScreenHeight { get; private set; } = 600;
    public int ScreenBits { get; private set; } = 8;
    public bool FullScreen { get; private set; } = false;
    public bool UseDoubleBuffering { get; private set; } = false;
    internal static bool ScreenFaded { get; private set; } = false;

    private readonly AssetManager _assetManager;
    public LegacyVideoManager(AssetManager assetManager)
    {
        _assetManager = assetManager;
        _ylookup = new uint[ScreenHeight];
    }

    private static int _scaleFactorX;
    private static int _scaleFactorY;

    private SDL.SDL_Color[] _currentPalette = new SDL.SDL_Color[256];

    private IntPtr _window;
    private IntPtr _renderer;
    private IntPtr _texture;
    private IntPtr _screen;
    private IntPtr _screenBuffer;
    private uint _screenPitch;
    private uint _bufferPitch;

    private uint[] _ylookup = null!;

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


    // public void DrawPic(int x, int y, string picName)
    // {
    //     var gfxManager = GraphicsManager.Instance;
    //     var graphic = gfxManager.GetGraphic(picName);
    //     
    //     //Remove this restriction.I want to be free to put in on any pixel I please
    //     //x &= ~7;
    //     
    //     MemToScreen(graphic.Data, graphic.Width, graphic.Height, x, y);
    // }

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
    //
    //     // TODO: Loop through each character in "text"
    //     // Or I can build the text in byte array size
    //     //      can send that once to MemToScreen
    //
    //     //MemToScreen(colorizedFont)
    //
    //     // get each character and print it to the byte[] pixels
    //
    // }

    // TODO: This should not be public as a byte[], only an "asset", which this handles
    public void MemToScreen(byte[] source, int width, int height, int x, int y)
    {
        MemToScreenScaledCoord(_screenBuffer, source, width, height, _scaleFactorX * x, _scaleFactorY * y);
    }

    // TODO: This should not obe public as a byte[], only an "asset lookup", which this handles
    public void MemToScreenScaledCoord(IntPtr screenBuffer, byte[] source, int width, int height, int destx, int desty)
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
                        if (col == 0xff) continue;

                        var xlength = sci + n + destx;
                        var ylength = scj + m + desty;
                        if (ylength > _ylookup.Length || (_ylookup[scj + m + desty] + xlength) > dest.Length) return;
                        dest[_ylookup[scj + m + desty] + sci + n + destx] = col;
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
        
        // SDL.SDL_Surface sdl_screenbuffer = (SDL.SDL_Surface)Marshal.PtrToStructure(screenBuffer, typeof(SDL.SDL_Surface));
        // GCHandle pinnedArray = GCHandle.Alloc(dest, GCHandleType.Pinned);
        // IntPtr dest_pointer = pinnedArray.AddrOfPinnedObject();
        // sdl_screenbuffer.pixels = dest_pointer;
        // Marshal.StructureToPtr(sdl_screenbuffer, screenBuffer, false);
        //
        // UnlockSurface(screenBuffer);
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
        SDL.SDL_Color[] pal = new SDL.SDL_Color[256];

        for (i = 0; i < 256; i++)
        {
            pal[i].r = red;
            pal[i].g = green;
            pal[i].b = blue;
        }

        SetPalette(pal, true);
    }

    private void SetPalette(SDL.SDL_Color[] palette, bool forceupdate)
    {
        Array.Copy(palette, _currentPalette, 256);

        if (ScreenBits == 8) // This shouldn't be a constant, it should be a gamesetting
        {
            // TODO: Null checking on these
            SDL.SDL_Surface sdl_screen = (SDL.SDL_Surface)Marshal.PtrToStructure(_screen, typeof(SDL.SDL_Surface));
            SDL.SDL_PixelFormat sdl_screen_format = (SDL.SDL_PixelFormat)Marshal.PtrToStructure(sdl_screen.format, typeof(SDL.SDL_PixelFormat));

            SDL_SetPaletteColors(sdl_screen_format.palette, palette, 0, 256);
        }

        SDL.SDL_Surface sdl_screenBuffer = (SDL.SDL_Surface)Marshal.PtrToStructure(_screenBuffer, typeof(SDL.SDL_Surface));
        SDL.SDL_PixelFormat sdl_screenBuffer_format = (SDL.SDL_PixelFormat)Marshal.PtrToStructure(sdl_screenBuffer.format, typeof(SDL.SDL_PixelFormat));

        SDL_SetPaletteColors(sdl_screenBuffer_format.palette, palette, 0, 256);

        if (forceupdate)
        {
            UpdateScreen(_screenBuffer);
        }
    }

    private void UpdateScreen(IntPtr surface)
    {
        SDL_BlitSurface(surface, ref Unsafe.NullRef<SDL.SDL_Rect>(), _screen, ref Unsafe.NullRef<SDL.SDL_Rect>());

        Present(_screen);
    }

    private void Present(IntPtr screen)
    {
        SDL.SDL_Surface sdl_screen = (SDL.SDL_Surface)Marshal.PtrToStructure(screen, typeof(SDL.SDL_Surface));
        SDL_UpdateTexture(_texture, ref Unsafe.NullRef<SDL.SDL_Rect>(), sdl_screen.pixels, ScreenWidth * sizeof(uint));
        SDL_RenderClear(_renderer);
        SDL_RenderCopy(_renderer, _texture, ref Unsafe.NullRef<SDL.SDL_Rect>(), ref Unsafe.NullRef<SDL.SDL_Rect>());
        SDL_RenderPresent(_renderer);
    }
    private IntPtr LockSurface(IntPtr surface)
    {
        if (SDL_MUSTLOCK(surface))
        {
            if (SDL_LockSurface(surface) < 0)
                return IntPtr.Zero;
        }

        SDL.SDL_Surface sdl_surface = (SDL.SDL_Surface)Marshal.PtrToStructure(surface, typeof(SDL.SDL_Surface));
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
        SDL.SDL_Color[] palette1 = new SDL.SDL_Color[256];
        SDL.SDL_Color[] palette2 = new SDL.SDL_Color[256];

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

    private void FadeIn(int start, int end, SDL.SDL_Color[] palette, int steps)
    {
        int i, j, delta;
        SDL.SDL_Color[] palette1 = new SDL.SDL_Color[256];
        SDL.SDL_Color[] palette2 = new SDL.SDL_Color[256];

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

// TODO: Convert this to a PLAYPAL file
internal static class GamePal
{
    private static SDL_Color RGB(byte red, byte green, byte blue)
    {
        return new SDL_Color
        {
            r = (byte)(red * 255 / 63),
            g = (byte)(green * 255 / 63),
            b = (byte)(blue * 255 / 63),
            a = 0
        };
    }

    internal static SDL_Color[] BasePalette = new SDL_Color[256]
    {
RGB(  0,  0,  0),RGB(  0,  0, 42),RGB(  0, 42,  0),RGB(  0, 42, 42),RGB( 42,  0,  0),
RGB( 42,  0, 42),RGB( 42, 21,  0),RGB( 42, 42, 42),RGB( 21, 21, 21),RGB( 21, 21, 63),
RGB( 21, 63, 21),RGB( 21, 63, 63),RGB( 63, 21, 21),RGB( 63, 21, 63),RGB( 63, 63, 21),
RGB( 63, 63, 63),RGB( 59, 59, 59),RGB( 55, 55, 55),RGB( 52, 52, 52),RGB( 48, 48, 48),
RGB( 45, 45, 45),RGB( 42, 42, 42),RGB( 38, 38, 38),RGB( 35, 35, 35),RGB( 31, 31, 31),
RGB( 28, 28, 28),RGB( 25, 25, 25),RGB( 21, 21, 21),RGB( 18, 18, 18),RGB( 14, 14, 14),
RGB( 11, 11, 11),RGB(  8,  8,  8),RGB( 63,  0,  0),RGB( 59,  0,  0),RGB( 56,  0,  0),
RGB( 53,  0,  0),RGB( 50,  0,  0),RGB( 47,  0,  0),RGB( 44,  0,  0),RGB( 41,  0,  0),
RGB( 38,  0,  0),RGB( 34,  0,  0),RGB( 31,  0,  0),RGB( 28,  0,  0),RGB( 25,  0,  0),
RGB( 22,  0,  0),RGB( 19,  0,  0),RGB( 16,  0,  0),RGB( 63, 54, 54),RGB( 63, 46, 46),
RGB( 63, 39, 39),RGB( 63, 31, 31),RGB( 63, 23, 23),RGB( 63, 16, 16),RGB( 63,  8,  8),
RGB( 63,  0,  0),RGB( 63, 42, 23),RGB( 63, 38, 16),RGB( 63, 34,  8),RGB( 63, 30,  0),
RGB( 57, 27,  0),RGB( 51, 24,  0),RGB( 45, 21,  0),RGB( 39, 19,  0),RGB( 63, 63, 54),
RGB( 63, 63, 46),RGB( 63, 63, 39),RGB( 63, 63, 31),RGB( 63, 62, 23),RGB( 63, 61, 16),
RGB( 63, 61,  8),RGB( 63, 61,  0),RGB( 57, 54,  0),RGB( 51, 49,  0),RGB( 45, 43,  0),
RGB( 39, 39,  0),RGB( 33, 33,  0),RGB( 28, 27,  0),RGB( 22, 21,  0),RGB( 16, 16,  0),
RGB( 52, 63, 23),RGB( 49, 63, 16),RGB( 45, 63,  8),RGB( 40, 63,  0),RGB( 36, 57,  0),
RGB( 32, 51,  0),RGB( 29, 45,  0),RGB( 24, 39,  0),RGB( 54, 63, 54),RGB( 47, 63, 46),
RGB( 39, 63, 39),RGB( 32, 63, 31),RGB( 24, 63, 23),RGB( 16, 63, 16),RGB(  8, 63,  8),
RGB(  0, 63,  0),RGB(  0, 63,  0),RGB(  0, 59,  0),RGB(  0, 56,  0),RGB(  0, 53,  0),
RGB(  1, 50,  0),RGB(  1, 47,  0),RGB(  1, 44,  0),RGB(  1, 41,  0),RGB(  1, 38,  0),
RGB(  1, 34,  0),RGB(  1, 31,  0),RGB(  1, 28,  0),RGB(  1, 25,  0),RGB(  1, 22,  0),
RGB(  1, 19,  0),RGB(  1, 16,  0),RGB( 54, 63, 63),RGB( 46, 63, 63),RGB( 39, 63, 63),
RGB( 31, 63, 62),RGB( 23, 63, 63),RGB( 16, 63, 63),RGB(  8, 63, 63),RGB(  0, 63, 63),
RGB(  0, 57, 57),RGB(  0, 51, 51),RGB(  0, 45, 45),RGB(  0, 39, 39),RGB(  0, 33, 33),
RGB(  0, 28, 28),RGB(  0, 22, 22),RGB(  0, 16, 16),RGB( 23, 47, 63),RGB( 16, 44, 63),
RGB(  8, 42, 63),RGB(  0, 39, 63),RGB(  0, 35, 57),RGB(  0, 31, 51),RGB(  0, 27, 45),
RGB(  0, 23, 39),RGB( 54, 54, 63),RGB( 46, 47, 63),RGB( 39, 39, 63),RGB( 31, 32, 63),
RGB( 23, 24, 63),RGB( 16, 16, 63),RGB(  8,  9, 63),RGB(  0,  1, 63),RGB(  0,  0, 63),
RGB(  0,  0, 59),RGB(  0,  0, 56),RGB(  0,  0, 53),RGB(  0,  0, 50),RGB(  0,  0, 47),
RGB(  0,  0, 44),RGB(  0,  0, 41),RGB(  0,  0, 38),RGB(  0,  0, 34),RGB(  0,  0, 31),
RGB(  0,  0, 28),RGB(  0,  0, 25),RGB(  0,  0, 22),RGB(  0,  0, 19),RGB(  0,  0, 16),
RGB( 10, 10, 10),RGB( 63, 56, 13),RGB( 63, 53,  9),RGB( 63, 51,  6),RGB( 63, 48,  2),
RGB( 63, 45,  0),RGB( 45,  8, 63),RGB( 42,  0, 63),RGB( 38,  0, 57),RGB( 32,  0, 51),
RGB( 29,  0, 45),RGB( 24,  0, 39),RGB( 20,  0, 33),RGB( 17,  0, 28),RGB( 13,  0, 22),
RGB( 10,  0, 16),RGB( 63, 54, 63),RGB( 63, 46, 63),RGB( 63, 39, 63),RGB( 63, 31, 63),
RGB( 63, 23, 63),RGB( 63, 16, 63),RGB( 63,  8, 63),RGB( 63,  0, 63),RGB( 56,  0, 57),
RGB( 50,  0, 51),RGB( 45,  0, 45),RGB( 39,  0, 39),RGB( 33,  0, 33),RGB( 27,  0, 28),
RGB( 22,  0, 22),RGB( 16,  0, 16),RGB( 63, 58, 55),RGB( 63, 56, 52),RGB( 63, 54, 49),
RGB( 63, 53, 47),RGB( 63, 51, 44),RGB( 63, 49, 41),RGB( 63, 47, 39),RGB( 63, 46, 36),
RGB( 63, 44, 32),RGB( 63, 41, 28),RGB( 63, 39, 24),RGB( 60, 37, 23),RGB( 58, 35, 22),
RGB( 55, 34, 21),RGB( 52, 32, 20),RGB( 50, 31, 19),RGB( 47, 30, 18),RGB( 45, 28, 17),
RGB( 42, 26, 16),RGB( 40, 25, 15),RGB( 39, 24, 14),RGB( 36, 23, 13),RGB( 34, 22, 12),
RGB( 32, 20, 11),RGB( 29, 19, 10),RGB( 27, 18,  9),RGB( 23, 16,  8),RGB( 21, 15,  7),
RGB( 18, 14,  6),RGB( 16, 12,  6),RGB( 14, 11,  5),RGB( 10,  8,  3),RGB( 24,  0, 25),
RGB(  0, 25, 25),RGB(  0, 24, 24),RGB(  0,  0,  7),RGB(  0,  0, 11),RGB( 12,  9,  4),
RGB( 18,  0, 18),RGB( 20,  0, 20),RGB(  0,  0, 13),RGB(  7,  7,  7),RGB( 19, 19, 19),
RGB( 23, 23, 23),RGB( 16, 16, 16),RGB( 12, 12, 12),RGB( 13, 13, 13),RGB( 54, 61, 61),
RGB( 46, 58, 58),RGB( 39, 55, 55),RGB( 29, 50, 50),RGB( 18, 48, 48),RGB(  8, 45, 45),
RGB(  8, 44, 44),RGB(  0, 41, 41),RGB(  0, 38, 38),RGB(  0, 35, 35),RGB(  0, 33, 33),
RGB(  0, 31, 31),RGB(  0, 30, 30),RGB(  0, 29, 29),RGB(  0, 28, 28),RGB(  0, 27, 27),
RGB( 38,  0, 34)

    };
}
