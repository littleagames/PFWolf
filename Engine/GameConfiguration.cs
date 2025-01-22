namespace Engine;

public class GameConfiguration
{
    /// <summary>
    /// The directory where to start looking for game packs
    /// </summary>
    public string BaseDirectory { get; set; } = "./";

    public Dimension ScreenSize { get; set; } = new Dimension(640, 400);
    
    public bool FullScreen { get; set; } = false;
    public byte ScreenBits { get; set; } = 8;
}