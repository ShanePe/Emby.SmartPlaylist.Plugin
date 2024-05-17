using System.Text.RegularExpressions;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Parsers.JsonValueParsers
{
    public class EmptyValueParser : JsonValueParser
    {
        public static Regex ParseRegEx = new Regex("{kind:empty,value:(.*)}", RegexOptions.IgnoreCase);

        public override bool TryParse(string value, out Value val)
        {
            val = null;
            var match = ParseRegEx.Match(value);
            if (match.Success)
            {
                val = EmptyValue.Create(match.Groups[1].Value);
                return true;
            }

            return false;
        }
    }
}