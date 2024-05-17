using System;
using SmartPlaylist.Extensions;

namespace SmartPlaylist.Domain.Values
{
    public class StringValue : EmptyableValue
    {
        public static readonly StringValue Default = new StringValue(string.Empty);

        public StringValue(string value)
        {
            Value = value ?? string.Empty;
        }

        public override string Kind => "string";

        public string Value { get; }

        internal override bool IsEmpty => IsNone || string.IsNullOrEmpty(Value);

        internal override string Friendly => Value;

        public static StringValue Create(string value)
        {
            return new StringValue(value);
        }

        public override string ToString()
        {
            return Value;
        }

        protected bool Equals(StringValue other)
        {
            return string.Equals(Value, other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((StringValue)obj);
        }

        public override int GetHashCode()
        {
            return Value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Value) : 0;
        }

        public static bool operator ==(StringValue left, StringValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StringValue left, StringValue right)
        {
            return !Equals(left, right);
        }

        public bool Contains(StringValue other)
        {
            return Value.Contains(other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool Is(StringValue other)
        {
            return Value.LastIndexOf(other.Value, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public bool StartsWith(StringValue other)
        {
            return Value.StartsWith(other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool EndsWith(StringValue other)
        {
            return Value.EndsWith(other.Value, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}