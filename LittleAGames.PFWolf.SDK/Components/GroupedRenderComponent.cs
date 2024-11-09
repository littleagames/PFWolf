namespace LittleAGames.PFWolf.SDK.Components;

public abstract class GroupedRenderComponent : RenderComponent
{
    public readonly List<RenderComponent> Components = new();
}