namespace LittleAGames.PFWolf.SDK.Models;

public class Player : Actor
{
    public Player(int tileX, int tileY, float angle, int health)
        : base(tileX, tileY, angle)
    {
        Health = health;
    }

    public int Health { get; private set; }
    
    public static event Action<int> OnHealthUpdated;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        OnHealthUpdated?.Invoke(damage);
    }
}