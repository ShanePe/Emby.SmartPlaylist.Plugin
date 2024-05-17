using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator.Operators.ListOperators.ComparableListValueOperators
{
    public class IsLessThanListValueOperator : OperatorGen<ListValue, ListValue>
    {
        public IsLessThanListValueOperator() : this(ListValue.Default)
        {
        }

        public IsLessThanListValueOperator(ListValue defaultListValue)
        {
            DefaultValue = defaultListValue;
        }

        public override string Name => "is less than";

        public override Value DefaultValue { get; }

        public override bool Compare(Value itemValue, Value value)
        {
            return Compare(itemValue as ListValue, value as ListValue);
        }

        public override bool Compare(ListValue itemValue, ListValue value)
        {
            return itemValue.NumValue < value.NumValue;
        }

        public override bool Compare(ArrayValue<ListValue> itemValues, ListValue value)
        {
            throw new System.NotImplementedException();
        }
    }
}