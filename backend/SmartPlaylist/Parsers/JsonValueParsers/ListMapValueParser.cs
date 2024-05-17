using System.Globalization;
using System.Text.RegularExpressions;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Parsers.JsonValueParsers
{
    public class ListMapValueParser : JsonValueParser
    {
        public static Regex ParseRegEx =
            new Regex("{map:(.*),value:(.*),kind:listMapValue}", RegexOptions.IgnoreCase);

        public override bool TryParse(string value, out Value val)
        {
            val = null;
            var match = ParseRegEx.Match(value);
            if (match.Success && match.Groups[2].Value is string strValue && match.Groups[1].Value is string strMap)
            {
                val = new ListMapValue(strValue, strMap);
                return true;
            }

            return false;
        }
    }
}