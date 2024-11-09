namespace LittleAGames.PFWolf.SDK.Components;

public class Stripe : Rectangle
{
    public byte StripeColor { get; }

    private Stripe(int y, byte stripeColor)
        : base(0, y, 320, 24, 0x00)
    {
        StripeColor = stripeColor;
        RawData = BuildRawData();
    }

    public byte[] RawData { get; set; } 

    public static Stripe Create(int y, byte stripeColor)
        => new Stripe(y, stripeColor);

    private byte[] BuildRawData()
    {
        byte[] data = Enumerable.Repeat(Color, Width * Height).ToArray();
        const int stripeY = 22;
        for (int x = 0; x < Width; x++)
        {
            data[stripeY*Width + x] = StripeColor;
        }

        return data;
    }
}