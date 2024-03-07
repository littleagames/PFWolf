using System.Text.RegularExpressions;

namespace Engine.Utilities;

internal class Scanner
{
    public MatchCollection Matches { get; private set; } = null!;
    public string String { get; private set; } = null!;
    public float Float { get; private set; } = 0;
    public int Number { get; private set; } = 0;

    private int _matchIndex = -1;

    public Scanner(string s)
    {
        ParseText(new StringReader(s));
    }

    public string GetString()
    {
        if (_matchIndex >= Matches.Count) return null!;

        if (_matchIndex < 0)
        {
            throw new Exception("No available words (unexpected end of file)");
        }

        String = Matches[_matchIndex++].Value;
        return String;
    }

    public void ParseText(StringReader reader)
    {
        var scannedLine = reader.ReadToEnd()?
                // Clean the text
                .Replace("\t", string.Empty)
                .Trim();

        var blockComments = @"/\*(.*?)\*/";
        var lineComments = @"//(.*?)\r?\n";
        var strings = @"""((\\[^\n]|[^""\n])*)""";
        var verbatimStrings = @"@(""[^""]*"")+";
        string noComments = Regex.Replace(scannedLine,
            blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
            me => {
                if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                    return me.Value.StartsWith("//") ? Environment.NewLine : "";
                // Keep the literal strings
                return me.Value;
            },
            RegexOptions.Singleline);

        var regexStr = @"^(?<=[^\\]|^)\"".*?(?<=[^\\])\""|(\\""|[\w\/\-])+|([\,{}()])+";
        Matches = Regex.Matches(noComments, regexStr);
        _matchIndex = 0;
    }

    public void UnGet()
    {
        _matchIndex--;
    }

    public bool CheckString(string name)
    {
        if (GetString() != null)
        {
            if (Compare(name))
            {
                return true;
            }

            UnGet();
        }

        return false;
    }

    public bool Compare(string value)
    {
        return String?.Equals(value, StringComparison.InvariantCultureIgnoreCase) ?? false;
    }

    public string MustGetString()
    {
        var line = GetString();
        if (line == null)
        {
            throw new Exception("Missing string (unexpected end of file)");
        }

        return line;
    }

    public void MustGetString(string value)
    {
        var line = MustGetString();
        if (Compare(value) == false)
        {
            throw new Exception($"Expected '{value}' but got '{line}'");
        }
    }

    public void MustGetFloat()
    {
        var line = GetString();
        if (line == null)
        {
            throw new Exception("Missing string (unexpected end of file)");
        }

        if (!float.TryParse(line, out float value))
        {
            throw new Exception($"Bad numeric constant '{line}'");
        }

        Float = value;
    }
    public void MustGetNumber()
    {
        var line = GetString();
        if (line == null)
        {
            throw new Exception("Missing string (unexpected end of file)");
        }

        if (!int.TryParse(line, out int value))
        {
            throw new Exception($"Bad numeric constant '{line}'");
        }

        Number = value;
    }
}
