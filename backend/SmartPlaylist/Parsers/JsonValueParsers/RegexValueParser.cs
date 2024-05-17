using System.Text.RegularExpressions;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Parsers.JsonValueParsers
{
    public class RegexValueParser : JsonValueParser
    {
        public static Regex ParseRegEx = new Regex("{caseSensitive:(.*),kind:regex,value:(.*)}", RegexOptions.IgnoreCase);

        public override bool TryParse(string value, out Value val)
        {
            val = null;
            var match = ParseRegEx.Match(value);
            if (match.Success)
            {
                bool cs = false;
                bool.TryParse(match.Groups[1].Value, out cs);
                val = RegexValue.Create(match.Groups[2].Value, cs);
                return true;
            }

            return false;
        }
    }
}