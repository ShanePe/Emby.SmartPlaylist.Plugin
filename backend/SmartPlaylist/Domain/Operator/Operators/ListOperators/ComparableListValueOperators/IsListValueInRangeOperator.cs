using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator.Operators.ListOperators.ComparableListValueOperators
{
    public class IsListValueInRangeOperator : OperatorGen<ListValue, ListValueRange>
    {
        public IsListValueInRangeOperator() : this(ListValueRange.Default)
        {
        }

        public IsListValueInRangeOperator(ListValueRange defaultListValue)
        {
            DefaultValue = defaultListValue;
        }

        public override string Name => "is in the range";

        public override Value DefaultValue { get; }

        public override bool Compare(Value itemValue, Value value)
        {
            return Compare(itemValue as ListValue, value as ListValueRange);
        }

        public override bool Compare(ListValue itemValue, ListValueRange value)
        {
            return itemValue.IsBetween(value);
        }

        public override bool Compare(ArrayValue<ListValue> itemValues, ListValueRange value)
        {
            throw new System.NotImplementedException();
        }
    }
}