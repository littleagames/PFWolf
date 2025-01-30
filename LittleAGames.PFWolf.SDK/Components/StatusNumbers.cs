namespace LittleAGames.PFWolf.SDK.Components;

public class StatusNumbers : RenderComponent
{
    private readonly Position _position;
    private readonly int _digits;
    private readonly List<Graphic> _digitGraphics = [];
    private const int AssetWidth = 8; // TODO: Find this info when asset is loaded? Or some type of metadata
    
    public int Value { get; set; }

    private const string BlankDigit = "n_blank";
    private readonly string[] _digitAssets = ["n_0", "n_1", "n_2", "n_3", "n_4", "n_5", "n_6", "n_7", "n_8", "n_9"];

    private StatusNumbers(Position position, int digits)
    {
        _position = position;
        _digits = digits;
    }

    public static StatusNumbers Create(Position position, int digits)
        => new StatusNumbers(position, digits);

    public override void OnStart()
    {
        for (var i = 0; i < _digits; i++)
        {
            // TODO: Digit width??
            _digitGraphics.Add(Graphic.Create(BlankDigit, _position.X+(AssetWidth*i), _position.Y));
        }

        Children.Add(_digitGraphics);
    }
    
    public override void OnUpdate()
    {
        var str = Value.ToString();
        var strLength = str.Length;
        var width = _digits;
        var x = 0;
        while (strLength < width)
        {
            _digitGraphics[x].SetAsset(BlankDigit);
            x++;
            width--;
        }

        var c = strLength <= width ? 0 : strLength - width;
        while (c < strLength)
        {
            _digitGraphics[x].SetAsset(_digitAssets[str[c]-'0']);
            x++;
            c++;
        }
    }
}