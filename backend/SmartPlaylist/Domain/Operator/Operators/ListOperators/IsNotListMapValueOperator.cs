using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator.Operators.ListOperators
{
    public class IsNotListMapValueOperator : IsListMapValueOperator
    {
        public IsNotListMapValueOperator() : this(ListMapValue.Default)
        {
        }

        public IsNotListMapValueOperator(ListMapValue defaultListValue) : base(defaultListValue)
        {
        }

        public override string Name => "is not";

        public override bool Compare(Value itemValue, Value value)
        {
            return !base.Compare(itemValue, value);
        }
    }
}