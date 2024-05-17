using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator.Operators.NumberOperators
{
    public class IsNumberOperator : Operator
    {
        public override string Name => "is";
        public override Value DefaultValue => NumberValue.Default;

        public override bool Compare(Value itemValue, Value value)
        {
            return itemValue?.Equals(value) ?? false;
        }

        public override bool Valueless => false;
    }
}