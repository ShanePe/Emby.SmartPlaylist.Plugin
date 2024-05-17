using System;
using System.Runtime.Serialization;

namespace SmartPlaylist.Domain.Values
{
    public class LastPeriodValue : Value
    {
        public static readonly LastPeriodValue Default = new LastPeriodValue(1, PeriodType.Weeks);

        public LastPeriodValue(int value, PeriodType periodType)
        {
            Value = value;
            PeriodType = periodType;
        }

        public override string Kind => "lastPeriod";

        public int Value { get; }
        public PeriodType PeriodType { get; }

        internal override string Friendly => $"Type: {PeriodType.ToString()}, Value: {Value}";

        public static LastPeriodValue Create(int value, PeriodType periodType)
        {
            return new LastPeriodValue(value, periodType);
        }

        public DateRangeValue ToDateRange(DateTimeOffset now)
        {
            DateTimeOffset fromDate;
            switch (PeriodType)
            {
                case PeriodType.Days:
                    fromDate = now.AddDays(-1 * Value);
                    break;
                case PeriodType.Weeks:
                    fromDate = now.AddDays(-1 * 7 * Value);
                    break;
                case PeriodType.Months:
                    fromDate = now.AddMonths(-1 * Value);
                    break;
            }

            return DateRangeValue.Create(fromDate, now);
        }

        protected bool Equals(LastPeriodValue other)
        {
            return Value == other.Value && PeriodType == other.PeriodType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((LastPeriodValue)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Value * 397) ^ (int)PeriodType;
            }
        }

        public static bool operator ==(LastPeriodValue left, LastPeriodValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LastPeriodValue left, LastPeriodValue right)
        {
            return !Equals(left, right);
        }
    }

    public enum PeriodType
    {
        [EnumMember(Value = "days")] Days = 1,
        [EnumMember(Value = "weeks")] Weeks = 2,
        [EnumMember(Value = "months")] Months = 3
    }
}