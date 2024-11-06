using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace LittleAGames.PFWolf.Common.Extensions;

public static class ImageSharpExtensions
{
    public static Color[] ToImageSharpColors(this System.Drawing.Color[] colors)
        => colors.Select(c => Color.FromRgb(c.R, c.G, c.B)).ToArray();
    
    public static Rgba32[] ToRgba32(this System.Drawing.Color[] colors)
        => colors.Select(c => new Rgba32(c.R, c.G, c.B)).ToArray();
    
    public static byte FindNearestColor(Rgba32 targetColor, List<Rgba32> colorPalette)
    {
        Rgba32 nearestColor = default;
        double nearestDistance = double.MaxValue;

        foreach (var color in colorPalette)
        {
            double distance = ColorDistance(targetColor, color);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestColor = color;
            }
        }

        return (byte)colorPalette.IndexOf(nearestColor);
    }

    private static double ColorDistance(Rgba32 color1, Rgba32 color2)
    {
        // Calculate the Euclidean distance between the two colors
        int rDiff = color1.R - color2.R;
        int gDiff = color1.G - color2.G;
        int bDiff = color1.B - color2.B;

        return Math.Sqrt(rDiff * rDiff + gDiff * gDiff + bDiff * bDiff);
    }
}