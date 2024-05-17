using System.Linq;
using SmartPlaylist.Domain.Operator.Operators.ListOperators;
using SmartPlaylist.Domain.Operator.Operators.ListOperators.ComparableListValueOperators;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Operator
{
    public static class OperatorsFactory
    {
        public static Operator[] CreateComparableListValueOperators(ListValue defaultListValue)
        {
            return CreateListValueOperators(defaultListValue).Concat(new Operator[]
            {
                new IsGreaterThanListValueOperator(defaultListValue),
                new IsLessThanListValueOperator(defaultListValue),
                new IsListValueInRangeOperator(ListValueRange.Create(defaultListValue, defaultListValue)),
            }).ToArray();
        }

        public static Operator[] CreateListValueOperators(ListValue defaultListValue)
        {
            return new Operator[]
            {
                new IsListValueOperator(defaultListValue),
                new IsNotListValueOperator(defaultListValue),
            };
        }

        public static Operator[] CreateListMapValueOperators(ListMapValue defaultListValue)
        {
            return new Operator[]
            {
                new IsListMapValueOperator(defaultListValue),
                new IsNotListMapValueOperator(defaultListValue),
            };
        }

    }
}