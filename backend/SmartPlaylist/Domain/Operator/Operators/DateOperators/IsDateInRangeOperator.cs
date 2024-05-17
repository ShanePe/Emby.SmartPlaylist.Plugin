using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator.Operators.DateOperators
{
    public class IsDateInRangeOperator : OperatorGen<DateValue, DateRangeValue>
    {
        public override string Name => "is in the range";
        public override Value DefaultValue => DateRangeValue.Default;

        public override bool Compare(Value itemValue, Value value)
        {
            return Compare(itemValue as DateValue, value as DateRangeValue);
        }

        public override bool Compare(DateValue itemValue, DateRangeValue value)
        {
            return itemValue.IsBetween(value);
        }

        public override bool Compare(ArrayValue<DateValue> itemValues, DateRangeValue value)
        {
            throw new System.NotImplementedException();
        }
    }
}