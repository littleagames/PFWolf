using System.Linq;

namespace Engine.Utilities
{
    internal class Scanner : StringReader
    {
        private bool _skipLineRead = false;
        public string? CurrentLine { get; private set; }

        public Scanner(string s) : base(s)
        {
        }

        public override string? ReadLine()
        {
            if (_skipLineRead)
            {
                _skipLineRead = false;
                return CurrentLine;
            }

            CurrentLine = base.ReadLine();
            return CurrentLine;
        }

        public string? ReadWord()
        {
            string? word = null;
            char? singleChar;
            while ((singleChar = (char)Read()).HasValue
                && !new List<char> { ' ' }.Contains(singleChar.Value))
            {
                word += singleChar.Value;
            }

            return word;
        }

        public bool CheckString(string word)
        {
            var line = ReadLine()?.Replace("\t", string.Empty);
            _skipLineRead = true;
            return line?.StartsWith(word, StringComparison.InvariantCultureIgnoreCase) ?? false;
            // todo "unget", go back one line in the

        }
    }
}
