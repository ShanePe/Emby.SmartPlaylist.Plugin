using System;

namespace SmartPlaylist.Domain.Values
{
    public class ListValue : Value
    {
        public static readonly ListValue Default = new ListValue(string.Empty);

        public ListValue(string value, float numValue = 0)
        {
            NumValue = numValue;
            Value = value;
        }

        public float NumValue { get; }

        public override string Kind => "listValue";

        public string Value { get; }

        internal override string Friendly => Value;

        public override string ToString()
        {
            return Value;
        }

        protected bool Equals(ListValue other)
        {
            return string.Equals(Value, other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ListValue)obj);
        }

        public override int GetHashCode()
        {
            return Value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Value) : 0;
        }

        public static bool operator ==(ListValue left, ListValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ListValue left, ListValue right)
        {
            return !Equals(left, right);
        }


        public static ListValue Create(string value, float numValue = 0)
        {
            return new ListValue(value, numValue);
        }


        public bool IsBetween(ListValueRange range)
        {
            return NumValue <= range.To.NumValue && NumValue >= range.From.NumValue;
        }
    }
}