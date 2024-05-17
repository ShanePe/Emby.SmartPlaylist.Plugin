using System.Globalization;
using System.Text.RegularExpressions;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Parsers.JsonValueParsers
{
    public class ListValueParser : JsonValueParser
    {
        public static Regex ParseRegEx =
            new Regex("{numValue:(.*),kind:listValue,value:(.*)}", RegexOptions.IgnoreCase);

        public static Regex ParseRegExWithoutNumValue =
            new Regex("{kind:listValue,value:(.*)}", RegexOptions.IgnoreCase);

        public override bool TryParse(string value, out Value val)
        {
            if (TryGetWithNumValue(value, out val)) return true;

            if (TryGetWithoutNumValue(value, out val)) return true;

            return false;
        }

        private static bool TryGetWithoutNumValue(string value, out Value val)
        {
            val = null;
            var match = ParseRegExWithoutNumValue.Match(value);
            if (match.Success && match.Groups[1].Value is string strValue)
            {
                val = new ListValue(strValue);
                return true;
            }

            return false;
        }

        private static bool TryGetWithNumValue(string value, out Value val)
        {
            val = null;
            var match = ParseRegEx.Match(value);
            if (match.Success && match.Groups[2].Value is string strValue && float.TryParse(match.Groups[1].Value,
                    NumberStyles.Any, CultureInfo.InvariantCulture, out var numValue))
            {
                val = new ListValue(strValue, numValue);
                return true;
            }

            return false;
        }
    }
}