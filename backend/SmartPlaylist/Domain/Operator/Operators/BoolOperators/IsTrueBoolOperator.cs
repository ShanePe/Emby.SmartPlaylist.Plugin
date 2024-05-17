using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator.Operators.BoolOperators
{
    public class IsTrueBoolOperator : OperatorGen<BoolValue, BoolValue>
    {
        public override string Name => "is true";
        public override Value DefaultValue => BoolValue.Default;

        public override bool Compare(Value itemValue, Value value)
        {
            return Compare(itemValue as BoolValue, value as BoolValue);
        }

        public override bool Compare(BoolValue itemValue, BoolValue value)
        {
            return itemValue.Value;
        }

        public override bool Compare(ArrayValue<BoolValue> itemValues, BoolValue value)
        {
            throw new System.NotImplementedException();
        }

        public override bool Valueless => true;
    }
}