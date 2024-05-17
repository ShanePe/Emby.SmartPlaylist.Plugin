using System;

namespace SmartPlaylist.Domain.Values
{
    public class ListMapValue : Value
    {
        public static readonly ListMapValue Default = new ListMapValue(string.Empty, string.Empty);

        public string Map { get; }
        public string Value { get; }
        public ListMapValue(string value, string map)
        {
            Map = map;
            Value = value;
        }

        public override string Kind => "listMapValue";

        internal override string Friendly => Map.Equals(Value) ? Value : $"{Map}=>{Value}";

        protected bool Equals(ListMapValue other)
        {
            return string.Equals(Map, other.Map, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ListMapValue)obj);
        }

        public override int GetHashCode()
        {
            return Value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Value) : 0;
        }

        public static bool operator ==(ListMapValue left, ListMapValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ListMapValue left, ListMapValue right)
        {
            return !Equals(left, right);
        }

        public static ListMapValue Create(string value, string map)
        {
            return new ListMapValue(value, map);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}