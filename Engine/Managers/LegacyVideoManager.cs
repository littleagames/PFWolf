using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using LittleAGames.PFWolf.SDK.Assets;
using LittleAGames.PFWolf.SDK.Components;
using SDL2;
using static SDL2.SDL;

namespace Engine.Managers;

public class LegacyVideoManager
{
    // public void DrawTextString(int startX, int startY, string text, FontName fontName, FontColor color)
    // {
    //     var gfxManager = GraphicsManager.Instance;
    //     var font = gfxManager.GetFont(fontName);
    //
    //     var printX = startX;
    //     var printY = startY;
    //
    //     foreach(char textChar in text)
    //     {
    //         var asciiIndex = (int)textChar;
    //         var fontChar = font.Characters[asciiIndex];
    //
    //         if (fontChar.Data.Length > 0)
    //         {
    //             var modifiedFontData = new byte[fontChar.Data.Length];
    //             for (var i = 0; i < fontChar.Data.Length; i++)
    //             {
    //                 var fontFlag = fontChar.Data[i] > 0;
    //                 modifiedFontData[i] = fontFlag ? (byte)color.Value : (byte)0xff;
    //             }
    //
    //             MemToScreen(modifiedFontData, fontChar.Width, fontChar.Height, printX, printY);
    //         }
    //
    //         if (textChar == '\n')
    //         {
    //             printX = startX;
    //             printY = printY + fontChar.Height;
    //             continue;
    //         }
    //
    //         printX += fontChar.Width;
    //     }
    //
    //     // TODO: Loop through each character in "text"
    //     // Or I can build the text in byte array size
    //     //      can send that once to MemToScreen
    //
    //     //MemToScreen(colorizedFont)
    //
    //     // get each character and print it to the byte[] pixels
    //
    // }
}
