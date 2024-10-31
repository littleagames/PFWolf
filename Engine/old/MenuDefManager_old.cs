// using System.Reflection;
//
// namespace Engine.Managers;
//
// internal class MenuDefManager_old
// {
//     private static volatile MenuDefManager_old? _instance = null;
//     private static object syncRoot = new object();
//
//     private ListMenuDescriptor _defaultListMenuSettings = new();
//     private Dictionary<MenuName, ListMenuDescriptor> _menuDescriptors = new();
//
//     private MenuDefManager_old()
//     {
//     }
//
//     /// <summary>
//     /// The instance of the singleton
//     /// safe for multithreading
//     /// </summary>
//     public static MenuDefManager_old Instance
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
//                         _instance = new MenuDefManager_old();
//                     }
//                 }
//             }
//
//             return _instance;
//         }
//     }
//
//     public ListMenuDescriptor GetMenu(MenuName menuName)
//     {
//         return _menuDescriptors[menuName];
//     }
//
//     public void ParseMenuDefs()
//     {
//         using var menuDefsFileStream = File.OpenRead($"{Constants.GameFilesDirectory}pfwolf-pk3\\menudef.txt");
//
//         using var streamReader = new StreamReader(menuDefsFileStream);
//
//         Scanner sr = new Scanner(streamReader.ReadToEnd());
//         
//         string? line;
//         while (sr.GetString() != null)
//         {
//             if (sr.Compare("LISTMENU"))
//             {
//                 ParseListMenu(sr);
//             }
//             else if (sr.Compare("DEFAULTLISTMENU"))
//             {
//                 DoParseListMenuBody(sr, _defaultListMenuSettings, insertIndex: -1);
//                 //if (DefaultListMenuSettings->mItems.Size() > 0)
//                // {
//                 //    I_FatalError("You cannot add menu items to the menu default settings.");
//                 //}
//             }
//         }
//     }
//
//     public void ParseListMenu(Scanner sr)
//     {
//         sr.MustGetString();
//         var menuDescriptor = new ListMenuDescriptor
//         {
//             MenuName = MenuName.FromString(sr.String),
//             SelectorOffsetX = _defaultListMenuSettings.SelectorOffsetX,
//             SelectorOffsetY = _defaultListMenuSettings.SelectorOffsetY,
//             Selector = _defaultListMenuSettings.Selector,
//             XPosition = _defaultListMenuSettings.XPosition,
//             YPosition = _defaultListMenuSettings.YPosition,
//             LineSpacing = _defaultListMenuSettings.LineSpacing,
//             Font = _defaultListMenuSettings.Font,
//             TextColor = _defaultListMenuSettings.TextColor,
//             BackgroundColor = _defaultListMenuSettings.BackgroundColor
//         };
//
//         DoParseListMenuBody(sr, menuDescriptor, insertIndex: -1);
//         ReplaceMenu(menuDescriptor);
//
//         //if (string.IsNullOrEmpty(line))
//         //{
//         //    throw new Exception("ParseListMenu: Unexpected end of file"); // TODO: Turn this into a better error handler, with lump name, and more details of what it was attempting to parse
//         //}
//     }
//
//     public void ReplaceMenu(ListMenuDescriptor menuDescriptor)
//     {
//         if (_menuDescriptors.TryGetValue(menuDescriptor.MenuName, out var existingDescriptor))
//         {
//             if (existingDescriptor.Protected)
//             {
//                 throw new Exception("Cannot replace protected menu");
//             }
//         }
//         // TODO: Check to see if there's an existing descriptor of the MenuName
//         // If so, check to see if it is protected
//         // Check to see if it is compatible with the existing menu
//         // Example, idk, i'll figure it out
//         _menuDescriptors[menuDescriptor.MenuName] = menuDescriptor;
//     }
//
//     public void DoParseListMenuBody(Scanner sr, ListMenuDescriptor menuDescriptor, int insertIndex)
//     {
//         sr.MustGetString("{");
//
//         while (!sr.CheckString("}"))
//         {
//             sr.MustGetString();
//
//             if (sr.Compare("else"))
//             {
//                 SkipSubBlock(sr);
//             }
//             else if (sr.Compare("ifgame"))
//             {
//                 if (!CheckSkipGameBlock(sr))
//                 {
//                     // recursively parse sub-block
//                     DoParseListMenuBody(sr, menuDescriptor, insertIndex);
//                 }
//             }
//             else if (sr.Compare("ifnotgame"))
//             {
//                 if (!CheckSkipGameBlock(sr, yes: false))
//                 {
//                     // recursively parse sub-block
//                     DoParseListMenuBody(sr, menuDescriptor, insertIndex);
//                 }
//             }
//             else if (sr.Compare("ifoption"))
//             {
//                 if (!CheckSkipOptionBlock(sr))
//                 {
//                     // recursively parse sub-block
//                     DoParseListMenuBody(sr, menuDescriptor, insertIndex);
//                 }
//             }
//             else if (sr.Compare("Class"))
//             {
//                 throw new NotImplementedException("Class in scripting has not be implemented yet.");
//             }
//             else if (sr.Compare("Selector"))
//             {
//                 sr.MustGetString();
//                 menuDescriptor.Selector = sr.String;
//                 sr.MustGetString(",");
//                 sr.MustGetNumber();
//                 menuDescriptor.SelectorOffsetX = sr.Number;
//                 sr.MustGetString(",");
//                 sr.MustGetNumber();
//                 menuDescriptor.SelectorOffsetY = sr.Number;
//             }
//             else if (sr.Compare("Linespacing"))
//             {
//                 sr.MustGetNumber();
//                 menuDescriptor.LineSpacing = sr.Number;
//             }
//             else if (sr.Compare("Position"))
//             {
//                 sr.MustGetNumber();
//                 menuDescriptor.XPosition = sr.Number;
//                 sr.MustGetString(",");
//                 sr.MustGetNumber();
//                 menuDescriptor.YPosition = sr.Number;
//             }
//             else if (sr.Compare("Font"))
//             {
//                 sr.MustGetString();
//                 menuDescriptor.Font = FontName.FromString(sr.String);
//                 sr.MustGetString(",");
//                 sr.MustGetNumber();
//                 menuDescriptor.TextColor = FontColor.FromInt(sr.Number);
//             }
//             else if (sr.Compare("BackgroundColor"))
//             {
//                 sr.MustGetNumber();
//                 menuDescriptor.BackgroundColor = FontColor.FromInt(sr.Number);
//             }
//             else
//             {
//                 // all item classes from which we know that they support sized scaling.
//                 // If anything else comes through here the option to swich scaling mode is disabled for this menu.
//                 //var compatibles = new List<string> { "StaticPatch", "StaticText", "StaticTextCentered", "TextItem", "PatchItem", "PlayerDisplay"}
//
//                 //if (!compatibles.Any(cp => line.StartsWith(cp)))
//                 //{
//                 //    sizeCompatible = false;
//                 //}
//
//                 var word = sr.String;
//
//                 bool success = false;
//                 
//                 var type = Type.GetType($"Engine.DataModels.ListMenuItem{word}");
//                 var ctor = type?.GetConstructors().FirstOrDefault();
//                 if (ctor == null)
//                 {
//                     throw new Exception($"{type} has no available constructor");
//                 }
//
//                 var ctorParams = ctor.GetParameters().ToList();
//
//                 var typedArgs = new List<object>();
//                 foreach (var param in ctorParams)
//                 {
//                     // Don't check comma before first parameter
//                     if (ctorParams.IndexOf(param) > 0)
//                     {
//                         sr.MustGetString(",");
//                     }
//
//                     switch (param.ParameterType)
//                     {
//                         case Type intType when intType == typeof(int):
//                             sr.MustGetNumber();
//                             typedArgs.Add(sr.Number);
//                             break;
//                         case Type floatType when floatType == typeof(float):
//                             sr.MustGetFloat();
//                             typedArgs.Add(sr.Float);
//                             break;
//                         case Type stringType when stringType == typeof(string):
//                             sr.MustGetString();
//                             typedArgs.Add(sr.String);
//                             break;
//                     }
//                 }
//
//                 var myObject = (ListMenuItem)ctor.Invoke(typedArgs.ToArray());
//                         
//                 // Handle additional properties
//                 if (myObject is ListMenuItemTextItem)
//                 {
//                     ((ListMenuItemTextItem)myObject).PositionX = menuDescriptor.XPosition;
//                     var index = menuDescriptor.Items.Count(md => md is ListMenuItemSelectable);
//                     ((ListMenuItemTextItem)myObject).PositionY = menuDescriptor.YPosition + menuDescriptor.LineSpacing * index;
//                     ((ListMenuItemTextItem)myObject).FontName = menuDescriptor.Font;
//                     ((ListMenuItemTextItem)myObject).TextColor = menuDescriptor.TextColor;
//                 }
//
//                 menuDescriptor.Items.Add(myObject);
//             }
//         }
//     }
//
//     public bool CheckSkipOptionBlock(Scanner sr)
//     {
//         var filter = false;
//         sr.MustGetString("(");
//         do
//         {
//             sr.MustGetString();
//             // TODO: Platform checks
//
//         } while (sr.CheckString(","));
//         sr.MustGetString(")");
//         // menu check options
//         // platform check (windows, apple, linux, etc
//
//         if (!filter)
//         {
//             SkipSubBlock(sr);
//             return !sr.CheckString("else");
//         }
//         
//         return false;
//     }
//
//     public bool CheckSkipGameBlock(Scanner sr, bool yes = true)
//     {
//         var filter = false;
//         sr.MustGetString("(");
//         do
//         {
//             sr.MustGetString();
//             filter |= CheckGame(sr.String);
//
//         } while (sr.CheckString(","));
//
//         sr.MustGetString(")");
//
//         if (filter != yes)
//         {
//             SkipSubBlock(sr);
//             return !sr.CheckString("else");
//         }
//
//         return false;
//     }
//
//     public void SkipSubBlock(Scanner sr)
//     {
//         sr.MustGetString("{");
//         SkipToEndOfBlock(sr);
//     }
//
//     public void SkipToEndOfBlock(Scanner sr)
//     {
//         int depth = 0;
//         while(true)
//         {
//             sr.MustGetString();
//             if (sr.Compare("{")) depth++;
//             else if (sr.Compare("}"))
//             {
//                 depth--;
//                 if (depth < 0) return;
//             }
//         }
//     }
//
//     public bool CheckGame(string game)
//     {
//         // TODO: Check if game is in list of system games?
//         // Wolf3D,Spear,Noah3D,ROTT
//         if (game.Equals("Wolf3D")) return true;
//         if (game.Equals("Wolfenstein3D")) return true;
//
//         if (game.Equals("Spear")) return true;
//         if (game.Equals("SpearOfDestiny")) return true;
//
//         return false;
//     }
// }
