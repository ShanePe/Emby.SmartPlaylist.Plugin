using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator.Operators.NumberOperators
{
    public class IsGreaterThanNumberOperator : OperatorGen<NumberValue, NumberValue>
    {
        public override string Name => "is greater than";
        public override Value DefaultValue => NumberValue.Default;

        public override bool Compare(Value itemValue, Value value)
        {
            return Compare(itemValue as NumberValue, value as NumberValue);
        }

        public override bool Compare(NumberValue itemValue, NumberValue value)
        {
            return itemValue.Value > value.Value;
        }

        public override bool Compare(ArrayValue<NumberValue> itemValues, NumberValue value)
        {
            throw new System.NotImplementedException();
        }
    }
}