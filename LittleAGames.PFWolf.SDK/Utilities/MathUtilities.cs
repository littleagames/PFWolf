namespace LittleAGames.PFWolf.SDK.Utilities;

public static class MathUtilities
{
    public static float ToRadians(this float degrees)
        => (float)(degrees * Math.PI / 180.0d);
}