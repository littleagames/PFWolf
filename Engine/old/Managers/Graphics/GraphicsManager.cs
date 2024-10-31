// using Engine.DataModels;
// using System.Xml.Linq;
//
// namespace Engine.Managers.Graphics;
//
// /// <summary>
// /// Reads all available graphics from all locations into a useable format by the engine
// /// </summary>
// internal class GraphicsManager : IGraphicsManager
// {
//     // TODO: Get VgaGraphics, /graphics from Pk3 file(s) all in the same format to store here
//     // TODO: Should all of it be in memory?
//
//     private static volatile GraphicsManager? _instance = null;
//     private static object syncRoot = new object();
//     private readonly IGraphicsManager vgaGraphicsManager;
//
//     private GraphicsManager(
//         IGraphicsManager vgaGraphicsManager) // TODO: Pass in an array?
//     {
//         this.vgaGraphicsManager = vgaGraphicsManager;
//     }
//
//     public static GraphicsManager Instance
//     {
//         get
//         {
//             if (_instance == null)
//             {
//                 // only create a new instance if one doesn't already exist.
//                 lock (syncRoot)
//                 {
//                     // use this lock to ensure that only one thread can access
//                     // this block of code at once.
//                     if (_instance == null)
//                     {
//                         _instance = new GraphicsManager(VgaGraphicsManager.Instance);
//                     }
//                 }
//             }
//
//             return _instance;
//         }
//     }
//
//     public void LoadDataFiles()
//     {
//         vgaGraphicsManager.LoadDataFiles();
//     }
//
//     public Graphic GetGraphic(string name)
//     {
//         return vgaGraphicsManager.GetGraphic(name);
//         // TODO: from list of available graphic packs (pk3, vgagraph), search in priority order,
//         // call that manager, and return the graphic in a manageable format
//     }
//
//     public Font GetFont(FontName fontName)
//     {
//         return vgaGraphicsManager.GetFont(fontName);
//     }
// }
