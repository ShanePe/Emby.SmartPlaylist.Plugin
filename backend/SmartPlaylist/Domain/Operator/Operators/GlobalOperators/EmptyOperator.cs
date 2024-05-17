using SmartPlaylist.Domain.Operator;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator.Operators.GlobalOperators
{
    public class EmptyOperator : Operator
    {
        public override string Name => "is empty";

        public override string Type => EmptyValue.Default.Kind;

        public override Value DefaultValue => EmptyValue.Default;

        public override bool CanCompare(Value itemValue, Value value)
        {
            return value is EmptyValue && typeof(EmptyableValue).IsAssignableFrom(itemValue.GetType());
        }
        public override bool Compare(Value itemValue, Value value)
        {
            return (itemValue is EmptyableValue eValue) ? eValue.IsEmpty : false;
        }

        public override bool Valueless => true;
    }
}