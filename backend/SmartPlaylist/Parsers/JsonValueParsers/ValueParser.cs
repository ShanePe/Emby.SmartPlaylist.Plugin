using System;
using System.Linq;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Parsers.JsonValueParsers
{
    public static class ValueParser
    {
        public static Value Parse(object inputObj)
        {
            Value value = null;

            switch (inputObj)
            {
                case string strValue:
                    DefinedJsonValueParsers.All.Any(x => x.TryParse(strValue, out value));
                    break;
                case Value validValue:
                    value = validValue;
                    break;
            }

            return value ?? Value.None;
        }
    }
}