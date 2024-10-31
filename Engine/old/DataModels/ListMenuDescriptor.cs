// using Engine.Managers;
//
// namespace Engine.DataModels;
//
// public class ListMenuDescriptor
// {
//     /// <summary>
//     /// Key of the menu that this data defines
//     /// </summary>
//     public MenuName MenuName { get; set; } = MenuName.Empty;
//     /// <summary>
//     /// Flag to prevent a menu from being modified
//     /// </summary>
//     public bool Protected { get; set; } = false;
//
//     /// <summary>
//     /// Texture name for the selector/cursor of the menu
//     /// </summary>
//     public string Selector { get; set; } = null!;
//     public int SelectorOffsetX { get; set; } = 0;
//     public int SelectorOffsetY { get; set; } = 0;
//     public int LineSpacing { get; set; } = 0;
//     public int XPosition { get; set; } = 0;
//     public int YPosition { get; set; } = 0;
//     public FontColor BackgroundColor { get;set; } = FontColor.FromByte(0x00);
//     public FontColor TextColor { get; set; } = FontColor.FromByte(0x00);
//     public FontName Font { get; set; } = FontName.Empty;
//     public List<ListMenuItem> Items { get; set; } = [];
//
//     public void Draw(VideoLayerManager_old vl)
//     {
//         vl.DrawBackground(BackgroundColor.GetByte());
//         Items.ForEach(component => component.Draw(vl));
//
//         vl.DrawPic(
//             (int)(XPosition + SelectorOffsetX),
//             (int)(YPosition + SelectorOffsetY),
//             Selector);
//
//         vl.UpdateScreen();
//     }
// }
