using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator.Operators.DateOperators
{
    public class IsDateOperator : Operator
    {
        public override string Name => "is";
        public override Value DefaultValue => DateValue.Default;

        public override bool Compare(Value itemValue, Value value)
        {
            return itemValue?.Equals(value) ?? false;
        }

        public override bool Valueless => false;
    }
}