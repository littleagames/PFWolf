namespace LittleAGames.PFWolf.SDK.Models;

public class Player : Actor
{
    private int _health;
    
    public Player(int tileX, int tileY, float angle, int health)
        : base(tileX, tileY, angle)
    {
        Health = health;
    }

    public int Health
    {
        get
        {
            return _health;
        }
        private set
        {
            _health = value;
            OnHealthUpdated?.Invoke(value);
        }
    }

    public static event Action<int> OnHealthUpdated;

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
}