namespace Engine.Managers;

internal class MenuDefManager
{
    private static volatile MenuDefManager? _instance = null;
    private static object syncRoot = new object();

    private ListMenuDescriptor _defaultListMenuSettings = new();
    private Dictionary<MenuName, ListMenuDescriptor> _menuDescriptors = new();

    private MenuDefManager()
    {
    }

    /// <summary>
    /// The instance of the singleton
    /// safe for multithreading
    /// </summary>
    public static MenuDefManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // only create a new instance if one doesn't already exist.
                lock (syncRoot)
                {
                    // use this lock to ensure that only one thread can access
                    // this block of code at once.
                    if (_instance == null)
                    {
                        _instance = new MenuDefManager();
                    }
                }
            }

            return _instance;
        }
    }

    public ListMenuDescriptor GetMenu(MenuName menuName)
    {
        return _menuDescriptors[menuName];
    }

    public void ParseMenuDefs()
    {
        using var menuDefsFileStream = File.OpenRead($"{Constants.GameFilesDirectory}pfwolf-pk3\\menudef.txt");

        using var streamReader = new StreamReader(menuDefsFileStream);

        Scanner sr = new Scanner(streamReader.ReadToEnd());
        
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            // TODO: This should get the next "Word"
            var splitLine = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (splitLine.Any() && (splitLine[0]?.StartsWith("LISTMENU", StringComparison.InvariantCultureIgnoreCase) ?? false))
            {
                ParseListMenu(splitLine[1], sr);
            }
            else if (line?.StartsWith("DEFAULTLISTMENU", StringComparison.InvariantCultureIgnoreCase) ?? false)
            {
                DoParseListMenuBody(sr, _defaultListMenuSettings, insertIndex: -1);
                //if (DefaultListMenuSettings->mItems.Size() > 0)
               // {
                //    I_FatalError("You cannot add menu items to the menu default settings.");
                //}
            }
        }
    }

    public void ParseListMenu(string line, Scanner sr)
    {
        var menuDescriptor = new ListMenuDescriptor
        {
            MenuName = MenuName.FromString(line.Trim('\"')), // TODO: Get "string", aka, check for quotations on start and end
            SelectorOffsetX = _defaultListMenuSettings.SelectorOffsetX,
            SelectorOffsetY = _defaultListMenuSettings.SelectorOffsetY,
            Selector = _defaultListMenuSettings.Selector,
            XPosition = _defaultListMenuSettings.XPosition,
            YPosition = _defaultListMenuSettings.YPosition,
            LineSpacing = _defaultListMenuSettings.LineSpacing
        };

        DoParseListMenuBody(sr, menuDescriptor, insertIndex: -1);
        ReplaceMenu(menuDescriptor);

        //if (string.IsNullOrEmpty(line))
        //{
        //    throw new Exception("ParseListMenu: Unexpected end of file"); // TODO: Turn this into a better error handler, with lump name, and more details of what it was attempting to parse
        //}
    }

    public void ReplaceMenu(ListMenuDescriptor menuDescriptor)
    {
        if (_menuDescriptors.TryGetValue(menuDescriptor.MenuName, out var existingDescriptor))
        {
            if (existingDescriptor.Protected)
            {
                throw new Exception("Cannot replace protected menu");
            }
        }
        // TODO: Check to see if there's an existing descriptor of the MenuName
        // If so, check to see if it is protected
        // Check to see if it is compatible with the existing menu
        // Example, idk, i'll figure it out
        _menuDescriptors[menuDescriptor.MenuName] = menuDescriptor;
    }

    public void DoParseListMenuBody(Scanner sr, ListMenuDescriptor menuDescriptor, int insertIndex)
    {
        var openBracketLine = sr.ReadLine()?.Replace("\t", string.Empty);
        if (!openBracketLine?.Equals("{") ?? false)
        {
            throw new Exception("DoParseMenuListBody: Missing { to start body");
        }

        string? line;
        while (((line = sr.ReadLine()) != null) && !line.Trim(new char[] {'\t'}).Equals("}")) // TODO: The } check won't work if it has tabs before
        {
            line = line.Replace("\t", string.Empty); // Strip starting tabs
            if (line.Equals(string.Empty)) continue;

            if (line.StartsWith("else", StringComparison.InvariantCultureIgnoreCase))
            {
                SkipSubBlock(sr);
            }
            else if (line.StartsWith("ifgame", StringComparison.InvariantCultureIgnoreCase))
            {
                if (!CheckSkipGameBlock(sr))
                {
                    // recursively parse sub-block
                    DoParseListMenuBody(sr, menuDescriptor, insertIndex);
                }
            }
            else if (line.StartsWith("ifnotgame", StringComparison.InvariantCultureIgnoreCase))
            {
                if (!CheckSkipGameBlock(sr, yes: false))
                {
                    // recursively parse sub-block
                    DoParseListMenuBody(sr, menuDescriptor, insertIndex);
                }
            }
            else if (line.StartsWith("ifoption", StringComparison.InvariantCultureIgnoreCase))
            {
                if (!CheckSkipOptionBlock(sr))
                {
                    // recursively parse sub-block
                    DoParseListMenuBody(sr, menuDescriptor, insertIndex);
                }
            }
            else if (line.StartsWith("Selector", StringComparison.InvariantCultureIgnoreCase))
            {
                var splitLine = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var textureName = splitLine[1].Replace("\"", string.Empty).ToString();

                var xOffset = float.Parse(splitLine[2]);
                var yOffset = float.Parse(splitLine[3]);

                menuDescriptor.Selector = textureName;
                menuDescriptor.SelectorOffsetX = xOffset;
                menuDescriptor.SelectorOffsetY = yOffset;
                //sc.MustGetString();
                //desc->mSelector = GetMenuTexture(sc.String);
                //sc.MustGetStringName(",");
                //sc.MustGetFloat();
                //desc->mSelectOfsX = sc.Float;
                //sc.MustGetStringName(",");
                //sc.MustGetFloat();
                //desc->mSelectOfsY = sc.Float;
            }
            else if (line.StartsWith("Linespacing", StringComparison.InvariantCultureIgnoreCase))
            {
                var splitLine = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var lineSpacing = int.Parse(splitLine[1]);
                menuDescriptor.LineSpacing = lineSpacing;
                //sc.MustGetNumber();
                //desc->mLinespacing = sc.Number;
            }
            else if (line.StartsWith("Position", StringComparison.InvariantCultureIgnoreCase))
            {
                var splitLine = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var xPos = float.Parse(splitLine[1]);
                var yPos = float.Parse(splitLine[2]);
                menuDescriptor.XPosition = xPos;
                menuDescriptor.YPosition = yPos;
                //sc.MustGetFloat();
                //desc->mXpos = sc.Float;
                //sc.MustGetStringName(",");
                //sc.MustGetFloat();
                //desc->mYpos = sc.Float;
            }
            else if (line.StartsWith("Stripe", StringComparison.InvariantCultureIgnoreCase))
            {
                // TODO: Set stripe? stuff
            }
            else if (line.StartsWith("Font", StringComparison.InvariantCultureIgnoreCase))
            {
                // TODO: Set font stuff
            }
            else
            {
                // all item classes from which we know that they support sized scaling.
                // If anything else comes through here the option to swich scaling mode is disabled for this menu.
                //var compatibles = new List<string> { "StaticPatch", "StaticText", "StaticTextCentered", "TextItem", "PatchItem", "PlayerDisplay"}

                //if (!compatibles.Any(cp => line.StartsWith(cp)))
                //{
                //    sizeCompatible = false;
                //}

                //var word = sr.ReadWord();

                // I want the first word only
                var splitLine = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var word = splitLine[0];
                bool success = false;
                var buildName = $"ListMenuItem{word}";
                var type = Type.GetType($"Engine.DataModels.{buildName}");
                var myObject = (ListMenuItem)Activator.CreateInstance(type);

            }
        }
    }

    public bool CheckSkipOptionBlock(Scanner sr)
    {
        var filter = false;
        // menu check options
        // platform check (windows, apple, linux, etc

        if (!filter)
        {
            SkipSubBlock(sr);
            return !sr.CheckString("else");
        }
        
        return false;
    }

    public bool CheckSkipGameBlock(Scanner sr, bool yes = true)
    {
        var line = sr.CurrentLine;
        var openPar = line.IndexOf("(");
        var closePar = line.IndexOf(")");
        if (openPar == -1 || closePar == -1 || openPar >= closePar)
        {
            throw new Exception("CheckSkipGameBlock: line missing ( or )");
        }

        var games = line.Substring(openPar+1, closePar - 1 - openPar).Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (games.Length == 0)
        {
            throw new Exception("CheckSkipGameBlock: identifers missing from between ()");
        }

        var filter = false;
        foreach (var game in games) {
            filter |= CheckGame(game);
        }

        if (filter != yes)
        {
            SkipSubBlock(sr);
            return !sr.CheckString("else");
        }

        return false;
    }

    public void SkipSubBlock(Scanner sr)
    {
        var openBracketLine = sr.ReadLine()?.Replace("\t", string.Empty);
        if (!openBracketLine?.Equals("{") ?? false)
        {
            throw new Exception("DoParseMenuListBody: Missing { to start body");
        }

        SkipToEndOfBlock(sr);
    }

    public void SkipToEndOfBlock(Scanner sr)
    {
        int depth = 0;
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            line = line.Replace("\t", string.Empty);
            if (line.StartsWith("{")) depth++;
            else if (line.StartsWith("}"))
            {
                depth--;
                if (depth < 0) return;
            }
        }
    }

    public bool CheckGame(string game)
    {
        // TODO: Check if game is in list of system games?
        // Wolf3D,Spear,Noah3D,ROTT
        if (game.Equals("Wolf3D")) return true;
        if (game.Equals("Wolfenstein3D")) return true;

        if (game.Equals("Spear")) return true;
        if (game.Equals("SpearOfDestiny")) return true;

        return false;
    }
}
