namespace LittleAGames.PFWolf.SDK.Utilities;

public static class MathUtilities
{
    public static float ToRadians(this float degrees)
        => (float)(degrees * Math.PI / 180.0d);

    public static float RadiansToInteger
        => (float)(3600 / 2 / Math.PI);
}