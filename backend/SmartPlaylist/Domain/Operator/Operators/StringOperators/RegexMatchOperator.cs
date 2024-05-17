using System.Linq;
using System.Text.RegularExpressions;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator.Operators.StringOperators
{
    public class RegexMatchOperator : Operator
    {
        public override string Name => "regular expression (match)";

        public override Value DefaultValue => RegexValue.Default;

        public override bool Compare(Value itemValue, Value value)
        {
            var strValue = value as RegexValue;
            var strItemValue = itemValue as StringValue;
            var arrayStrItemValue = itemValue as ArrayValue<StringValue>;

            return strItemValue != null
                ? CompareStrings(strItemValue, strValue)
                : CompareArrayToString(arrayStrItemValue, strValue);
        }

        public override bool CanCompare(Value itemValue, Value value)
        {
            return !value.IsNone && (itemValue is StringValue ||
                                     itemValue is ArrayValue<StringValue> ||
                                     itemValue is RegexValue ||
                                     itemValue is ArrayValue<RegexValue>);
        }

        public bool CompareArrayToString(ArrayValue<StringValue> itemValue, RegexValue value)
        {
            return itemValue.Values.Any(x => IsMatch(x.Value, value.Value, value.CaseSensitive));
        }

        public bool CompareStrings(StringValue itemValue, RegexValue value)
        {
            return IsMatch(itemValue.Value, value.Value, value.CaseSensitive);
        }

        private bool IsMatch(string value, string pattern, bool caseSensitive)
        {
            return caseSensitive ?
                new Regex(pattern).Match(value).Success :
                new Regex(pattern, RegexOptions.IgnoreCase).Match(value).Success;
        }

        public override bool Valueless => false;
    }
}