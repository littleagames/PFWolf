namespace LittleAGames.PFWolf.SDK.Components;

public class Stripe : GroupedRenderComponent
{
    private Stripe(int y, byte stripeColor)
        : base()
    {
        Components.Add(Rectangle.Create(0, y, 320, 24, 0x00));
        Components.Add(Rectangle.Create(0, y+22, 320, 1, stripeColor));
    }

    public static Stripe Create(int y, byte stripeColor)
        => new Stripe(y, stripeColor);
}