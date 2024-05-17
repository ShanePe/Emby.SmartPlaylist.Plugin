using System.Linq;
using SmartPlaylist.Domain.Values;
using SmartPlaylist.Extensions;

namespace SmartPlaylist.Domain.Operator
{
    public static class DefinedOperators
    {
        public static readonly Operator[] All = typeof(Operator).Assembly.FindAndCreateDerivedTypes<Operator>()
            .OrderBy(x => x.Name).ToArray();

        public static readonly Operator[] StringOperators =
            All.Where(x =>
                x.Type == StringValue.Default.Kind ||
                x.Type == EmptyValue.Default.Kind ||
                x.Type == RegexValue.Default.Kind).ToArray();

        public static readonly Operator[] NumberOperators =
            All.Where(x =>
                x.Type == NumberValue.Default.Kind ||
                x.Type == NumberRangeValue.Default.Kind ||
                x.Type == EmptyValue.Default.Kind).ToArray();

        public static readonly Operator[] DateOperators =
        All.Where(x =>
            x.Type == DateRangeValue.Default.Kind ||
            x.Type == DateValue.Default.Kind ||
            x.Type == LastPeriodValue.Default.Kind ||
            x.Type == EmptyValue.Default.Kind).ToArray();

        public static readonly Operator[] BoolValueOperators =
            All.Where(x =>
                x.Type == BoolValue.Default.Kind ||
                x.Type == EmptyValue.Default.Kind).ToArray();
    }
}