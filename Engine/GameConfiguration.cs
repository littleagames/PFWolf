namespace Engine;

public class GameConfiguration
{
    /// <summary>
    /// The directory where to start looking for game packs
    /// </summary>
    public string BaseDirectory { get; set; } = "./";
    
    /// <summary>
    /// Asset name of the default game palette
    /// </summary>
    [Obsolete("Use gamepack asset value")]
    public string GamePalette { get; set; } = "wolfpal";

    public Dimension ScreenSize { get; set; } = new Dimension(640, 400);
    
    public bool FullScreen { get; set; } = false;
    public byte ScreenBits { get; set; } = 8;
    
    [Obsolete("Use gamepack asset value")]
    public string StartingScene { get; set; } = "wolf3d:SignonScene";
}