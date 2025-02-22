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
	
    public virtual void T_Stand()
    {
	    return;
    }
	
    public virtual void T_Path() {
    }
	
    public virtual void T_Chase() {
    }
	
    public virtual void A_Shoot() {
    }
	
    public virtual void T_Pain() {
        // TODO: This will determine whether or not to skip this frame, or change state
        // Doing one or the other action will run frame 1 or 2.
    }
}