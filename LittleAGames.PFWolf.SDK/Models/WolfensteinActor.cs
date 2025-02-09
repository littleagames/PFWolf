namespace LittleAGames.PFWolf.SDK.Models;

public class WolfensteinActor : Actor
{
	public WolfensteinActor(int tileX, int tileY, float angle) 
		: base(tileX, tileY, angle)
	{
	}

	public virtual void A_DeathScream() {
    }
	
    public virtual void T_Stand() {
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