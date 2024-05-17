using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator.Operators.NumberOperators
{
    public class IsInTheRangeNumberOperator : OperatorGen<NumberValue, NumberRangeValue>
    {
        public override string Name => "is in the range";
        public override Value DefaultValue => NumberRangeValue.Default;

        public override bool Compare(Value itemValue, Value value)
        {
            return Compare(itemValue as NumberValue, value as NumberRangeValue);
        }

        public override bool Compare(NumberValue itemValue, NumberRangeValue value)
        {
            return itemValue.IsInRange(value);
        }

        public override bool Compare(ArrayValue<NumberValue> itemValues, NumberRangeValue value)
        {
            throw new System.NotImplementedException();
        }
    }
}