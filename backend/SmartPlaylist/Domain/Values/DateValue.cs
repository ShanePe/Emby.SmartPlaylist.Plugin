using System;

namespace SmartPlaylist.Domain.Values
{
    public class DateValue : EmptyableValue
    {
        public static readonly DateValue Default = new DateValue(DateTimeOffset.Now);

        public DateValue(DateTimeOffset value)
        {
            Value = value.Date;
        }

        public override string Kind => "date";

        public DateTimeOffset Value { get; }

        internal override bool IsEmpty => IsNone;
        internal override string Friendly => Value.ToString();


        protected bool Equals(DateValue other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((DateValue)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(DateValue left, DateValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DateValue left, DateValue right)
        {
            return !Equals(left, right);
        }


        public static Value Create(DateTimeOffset date)
        {
            return new DateValue(date);
        }

        public bool IsBetween(DateRangeValue dateRange)
        {
            return Value >= dateRange.From && Value <= dateRange.To;
        }
    }
}