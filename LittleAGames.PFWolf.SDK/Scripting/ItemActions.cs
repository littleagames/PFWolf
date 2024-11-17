namespace LittleAGames.PFWolf.SDK.Scripting;

public class ItemActions : RunnableBase
{
    public ActorActions Player { get; } = new();
    public SoundActions Sounds { get; } = new();
}

public class ActorActions : RunnableBase
{
    public int Health { get; set; } = 0;
    public int MaxHealth { get; set; } = 100;

    public void GiveHealth(int value)
    {
        Health += value;
        if (Health > MaxHealth)
            Health = MaxHealth;
    }
}

public class SoundActions : RunnableBase
{
    public void Play(string soundName)
    {
        throw new NotImplementedException();
    }
}