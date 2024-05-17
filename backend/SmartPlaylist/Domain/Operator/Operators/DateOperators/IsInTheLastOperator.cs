using System;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator.Operators.DateOperators
{
    public class IsInTheLastOperator : OperatorGen<DateValue, LastPeriodValue>
    {
        public override string Name => "is in the last";
        public override Value DefaultValue => LastPeriodValue.Default;

        public override bool Compare(Value itemValue, Value value)
        {
            return Compare(itemValue as DateValue, value as LastPeriodValue);
        }

        public override bool Compare(DateValue itemValue, LastPeriodValue value)
        {
            return itemValue.IsBetween(value.ToDateRange(DateTimeOffset.Now));
        }

        public override bool Compare(ArrayValue<DateValue> itemValues, LastPeriodValue value)
        {
            throw new NotImplementedException();
        }
    }
}