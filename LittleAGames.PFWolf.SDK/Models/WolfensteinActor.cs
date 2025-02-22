namespace LittleAGames.PFWolf.SDK.Models;

public class WolfensteinActor : Actor
{
	public WolfensteinActor(int x, int y) : base(x, y)
	{
	}
	
	public WolfensteinActor(int x, int y, float angle)
		: base(x, y, angle)
	{
	}
	
	public WolfensteinActor(int x, int y, float angle, string beginningState)
		: base(x, y, angle, beginningState)
	{
	}

	public virtual void A_DeathScream() {
    }
	
    public virtual void T_Stand(float deltaTime)
    {
	    return;
    }
	
    public virtual void T_Path(float deltaTime)
    {
	    var move = Speed * deltaTime;
    }
	
    public virtual void T_Chase(float deltaTime) {
    }
	
    public virtual void A_Shoot(float deltaTime) {
    }
	
    public virtual void T_Pain(float deltaTime) {
        // TODO: This will determine whether or not to skip this frame, or change state
        // Doing one or the other action will run frame 1 or 2.
    }
}