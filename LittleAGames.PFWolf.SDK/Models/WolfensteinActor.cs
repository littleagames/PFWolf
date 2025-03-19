namespace LittleAGames.PFWolf.SDK.Models;

public class WolfensteinActor : Actor
{
	private float _distance;
	
	public WolfensteinActor(int x, int y) : base(x, y)
	{
		_distance = 1 << 16;
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

    public virtual void T_Path()
    {
	    Move(Speed, Angle);
	    return;
	    var move = Speed * 1;
	    while (move > 0)
	    {
		    if (_distance < 0)
		    {
			    // OpenDoor();
			    _distance = 1 << 16;
		    }
		    
		    if (move < _distance)
		    {
			    Move(move);
			    break;
		    }
		    
		    X = ((int)TileX<<16)+(1<<16)/2;
		    Y = ((int)TileY<<16)+(1<<16)/2;
		    move -= _distance;
		    
		    SelectPathDir();

		    if (Direction == Direction.NoDirection)
			    return;	// All movement is blocked
	    }
	}

    private void SelectPathDir()
    {
	    //uint spot;

	    //spot = MAPSPOT(ob->tilex,ob->tiley,1)-ICONARROWS;

	    // if (spot<8)
	    // {
		   //  // new direction
		   //  ob->dir = spot;
	    // }

	    _distance = 1 << 16;

	    //if (!TryWalk (ob))
		//    ob->dir = nodir;
    }

    private void Move(float move)
    {
	    float newX = X;
	    float newY = Y;

	    switch (Direction)
	    {
		    case Direction.North:
			    newY -= move;
			    break;
		    case Direction.NorthEast:
			    newX += move;
			    newY -= move;
			    break;
		    case Direction.East:
			    newX += move;
			    break;
			case Direction.SouthEast:
			    newX += move;
			    newY += move;
			    break;
			case Direction.South:
			    newY += move;
			    break;
			case Direction.SouthWest:
			    newX -= move;
			    newY += move;
			    break;
			case Direction.West:
				newX -= move;
				break;
			case Direction.NorthWest:
			    newX -= move;
				newY -= move;
				break;
			case Direction.NoDirection:
			    return;
	    }
	    
	    // check to make sure it's not on top of the player
	    
	    X = (int)newX;
	    Y = (int)newY;
	    _distance -= move;
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