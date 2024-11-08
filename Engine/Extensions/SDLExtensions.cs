using System.Drawing;
using LittleAGames.PFWolf.SDK.Assets;
using SDL2;

namespace Engine.Extensions;

public static class SDLExtensions
{
    internal static SDL.SDL_Color[] ToSDLPalette(this Color[] colors)
        => colors.Select(c => new SDL.SDL_Color
        {
            r = c.R,
            g = c.G,
            b = c.B,
            a = c.A
        }).ToArray();
    internal static SDL.SDL_Color[] ToSDLPalette(this byte[] data)
    {
        if (data.Length != 768) // 256*3 bytes
        {
            throw new ArgumentException("Invalid SDL palette data");
        }

        var paletteColors = new SDL.SDL_Color[256];
        for (int i = 0; i < 256; i++)
        {
            paletteColors[i] = new SDL.SDL_Color
            {
                r = data[(i * 3)],
                g = data[(i * 3) + 1],
                b = data[(i * 3) + 2],
            };
        }

        return paletteColors;
    }

    internal static SDL.SDL_Color[] ToSDLPalette(this PaletteAsset asset)
        => asset.RawData.ToSDLPalette();
    
}