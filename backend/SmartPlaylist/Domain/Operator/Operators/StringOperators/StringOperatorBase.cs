using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator.Operators.StringOperators
{
    public abstract class StringOperatorBase : Operator
    {
        public override Value DefaultValue => StringValue.Default;

        public abstract bool CompareStrings(StringValue itemValue, StringValue value);
        public abstract bool CompareArrayToString(ArrayValue<StringValue> itemValue, StringValue value);

        public override bool Compare(Value itemValue, Value value)
        {
            var strValue = value as StringValue;
            var strItemValue = itemValue as StringValue;
            var arrayStrItemValue = itemValue as ArrayValue<StringValue>;

            return strItemValue != null
                ? CompareStrings(strItemValue, strValue)
                : CompareArrayToString(arrayStrItemValue, strValue);
        }

        public override bool CanCompare(Value itemValue, Value value)
        {
            return value is StringValue && (itemValue is StringValue || itemValue is ArrayValue<StringValue>);
        }

        public override bool Valueless => false;
    }
}