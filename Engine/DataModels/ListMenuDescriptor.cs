namespace Engine.DataModels;

public class ListMenuDescriptor
{
    /// <summary>
    /// Key of the menu that this data defines
    /// </summary>
    public MenuName MenuName { get; set; } = MenuName.Empty;
    /// <summary>
    /// Flag to prevent a menu from being modified
    /// </summary>
    public bool Protected { get; set; } = false;

    /// <summary>
    /// Texture name for the selector/cursor of the menu
    /// </summary>
    public string Selector { get; set; } = null!;
    public float SelectorOffsetX { get; set; } = 0;
    public float SelectorOffsetY { get; set; } = 0;
    public int LineSpacing { get; set; } = 0;
    public float XPosition { get; set; } = 0;
    public float YPosition { get; set; } = 0;
}
