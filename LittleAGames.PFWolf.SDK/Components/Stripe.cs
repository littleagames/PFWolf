namespace LittleAGames.PFWolf.SDK.Components;

public class Stripe : RenderComponent
{
    private Stripe(int y, byte stripeColor)
        : base()
    {
        Children.Add(Rectangle.Create(0, y, 320, 24, 0x00));
        Children.Add(Rectangle.Create(0, y+22, 320, 1, stripeColor));
    }

    public static Stripe Create(int y, byte stripeColor)
        => new Stripe(y, stripeColor);
}