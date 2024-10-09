namespace LittleAGames.PFWolf.SDK;

public abstract class RunnableBase
{
    public virtual void Start()
    {
        PfConsole.Log("Base Start");
    }

    public virtual void Execute()
    {
        PfConsole.Log("Base Execute");
    }
    public virtual void Update()
    {
        PfConsole.Log("Base Update");
    }

    public virtual void Destroy()
    {
        PfConsole.Log("Base Destroy");
    }
}
