namespace LittleAGames.PFWolf.ExternalPk3ModPack.Actions;

public class BonusItemActions : ItemActions
{
    public void DogFood() {
        if (Player.Health == 100)
            return;
			
        Sounds.Play("health1");
        Player.GiveHealth(4);
    }
}