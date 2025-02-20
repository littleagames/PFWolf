public class Guard : WolfensteinActor
{
    public Guard(int tileX, int tileY) : base(tileX, tileY)
    {
    }
    
    public Guard(int tileX, int tileY, float angle) : base(tileX, tileY, angle)
    {
    }
    
    public Guard(int tileX, int tileY, float angle, string beginningState) : base(tileX, tileY, angle, beginningState)
    {
    }
}