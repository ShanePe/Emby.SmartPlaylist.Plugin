using System.Linq;
using System.Text.RegularExpressions;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator.Operators.StringOperators
{
    public class RegexNoMatchOperator : RegexMatchOperator
    {
        public override string Name => "regular expression (no match)";
        public override bool Compare(Value itemValue, Value value)
        {
            return !base.Compare(itemValue, value);
        }
    }
}