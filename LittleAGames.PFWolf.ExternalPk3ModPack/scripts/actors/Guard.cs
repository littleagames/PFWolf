public class Guard : WolfensteinActor
{
    public Guard(int tileX, int tileY) : base(tileX, tileY)
    {
        Speed = 512;
    }
    
    public Guard(int tileX, int tileY, float angle) : base(tileX, tileY, angle)
    {
        Speed = 512;
    }
    
    public Guard(int tileX, int tileY, float angle, string beginningState) : base(tileX, tileY, angle, beginningState)
    {
        Speed = 512;
    }
}