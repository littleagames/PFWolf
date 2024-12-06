namespace LittleAGames.PFWolf.SDK.Utilities;

public static class AngleUtilities
{
    public static float FixAngle(float a)
    {
        if (a > 359)
        {
            a-=360;
        }

        if (a < 0)
        {
            a+=360;
        } 
        
        return a;
    }
}