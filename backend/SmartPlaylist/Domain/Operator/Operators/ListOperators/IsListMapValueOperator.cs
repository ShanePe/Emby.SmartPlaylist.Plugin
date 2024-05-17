using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator.Operators.ListOperators
{
    public class IsListMapValueOperator : OperatorGen<ListMapValue, ListMapValue>
    {
        public IsListMapValueOperator() : this(ListMapValue.Default)
        {
        }

        public IsListMapValueOperator(ListMapValue defaultListValue)
        {
            DefaultValue = defaultListValue;
        }

        public override string Name => "is";
        public override Value DefaultValue { get; }

        public override bool Compare(Value itemValue, Value value)
        {
            return Compare(itemValue as ListMapValue, value as ListMapValue);
        }

        public override bool Compare(ListMapValue itemValue, ListMapValue value)
        {
            return itemValue?.Equals(value) ?? false;
        }

        public override bool Compare(ArrayValue<ListMapValue> itemValues, ListMapValue value)
        {
            throw new System.NotImplementedException();
        }
    }
}