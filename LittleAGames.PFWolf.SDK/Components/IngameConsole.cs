namespace LittleAGames.PFWolf.SDK.Components;

public class IngameConsole : RenderComponent
{
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }
    public byte BackgroundColor { get; }
    
    public byte ForegroundColor { get; }

    public bool IsActive => _state != ConsoleState.Closed;

    private ConsoleState _state = ConsoleState.Closed;
    private Rectangle _backdrop;
    private Text _typeableText;
    private Stack<string> _previousMessages = new();
    private List<Text> _previousMessageTexts = new(MaxPreviousMessages);
    private const int MaxPreviousMessages = 7;
    
    private IngameConsole(int x, int y, int width, int height, byte backgroundColor)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        BackgroundColor = backgroundColor;
    }

    public static IngameConsole Create(int x, int y, int width, int height, byte color)
        => new(x, y, width, height, color);

    public void ToggleState()
    {
        if (_state == ConsoleState.Closed)
            _state = ConsoleState.Open;
        else
            _state = ConsoleState.Closed;
    }
    
    public override void OnStart()
    {
        base.OnStart();
        _backdrop = Rectangle.Create(X, Y, Width, Height, BackgroundColor);
        _backdrop.Children.Add(Rectangle.Create(X, Height-14,Width, 1, 0x0f));
        _typeableText = Text.Create(">", X+1, Height-12, "SmallFont", 0x0f);
        Children.Add(_backdrop);
        Children.Add(_typeableText);
        for (int i = 0; i < MaxPreviousMessages; i++)
        {
            _previousMessageTexts.Add(Text.Create("", X+1, Y+(i*12), "SmallFont", 0x0f));
        }
        Children.Add(_previousMessageTexts.ToHashSet());
    }

    public override void OnUpdate()
    {
        // On opening, animate
        // On open, listen for inputs???
        // On closing, animate
        // on close, remain idle
        
        Hidden = _state == ConsoleState.Closed;

        while (_previousMessages.Count > MaxPreviousMessages)
        {
            _previousMessages.Pop();
            UpdateConsoleText();
        }
    }

    public void Listen(InputComponent input)
    {
        // TODO: Take keys pressed, and add them to string
        var keys = new List<Keys> { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, 
            Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.M, Keys.N,
            Keys.O, Keys.P, Keys.Q, Keys.R,Keys.S, Keys.T, Keys.U, Keys.V,
            Keys.W, Keys.X, Keys.Y, Keys.Z };
        foreach (var key in keys)
        {
            if (input.IsKeyDown(key))
            {
                _typeableText.String += key.ToString();
                Console.WriteLine($"Console Input: {_typeableText.String}");
                input.ClearKeysDown();
            }
        }

        if (input.IsKeyDown(Keys.Space))
        {
            _typeableText.String += " ";
            input.ClearKeysDown();
        }
        
        if (input.IsKeyDown(Keys.Backspace))
        {
            _typeableText.String = _typeableText.String.Substring(0, _typeableText.String.Length - 1);
            input.ClearKeysDown();
        }
        
        if (input.IsKeyDown(Keys.Return))
        {
            _previousMessages.Push(_typeableText.String);
            // TODO: Evaluate()
            var commands = _typeableText.String.Split(" ");
            if (commands.Length > 0)
                _previousMessages.Push($"Invalid verb '{commands.First()}'");
            // End evaluate
            UpdateConsoleText();
            
            _typeableText.String = ">";
            Console.WriteLine($"Console Input: {_typeableText.String}");
            input.ClearKeysDown();

            UpdateConsoleText();
        }
    }

    private void UpdateConsoleText()
    {
        var i = 0;
        foreach (var text in _previousMessages)
        {
            if (i >= MaxPreviousMessages)
                continue;
            _previousMessageTexts[MaxPreviousMessages - i - 1].String = text;
            i++;
        }
    }
}

public enum ConsoleState
{
    Closed,
    Opening,
    Open,
    Closing
}